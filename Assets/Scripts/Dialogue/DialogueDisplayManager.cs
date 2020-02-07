using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueDisplayManager : Singleton<DialogueDisplayManager>
{
    [SerializeField] private float characterDisplayWaitTime = 0.1f;
    [SerializeField] private float characterDisplayWaitTimeMultiplier = 0.3f;

    public Dictionary<DialogueSpeaker, DialogueDisplay> dialogueDisplays = new Dictionary<DialogueSpeaker, DialogueDisplay>();

    public UnityEvent OnConversationBegin = new UnityEvent();
    public UnityEvent OnConversationEnd = new UnityEvent();
    public UnityEvent OnDialogueLineEnd = new UnityEvent();

    private Conversation conversationToDisplay;
    private Dialogue currentDialogue;
    private DialogueSpeaker previousSpeaker;
    private DialogueDisplay currentDisplay;
    private string nextLine;
    private WaitForSeconds waitTime;
    private State currentState;

    private void Start()
    {
        waitTime = new WaitForSeconds(characterDisplayWaitTime);
    }

    public void DisplayConversation(Conversation conversation)
    {
        // Set the conversation
        conversationToDisplay = conversation;

        // We make sure that if the conversation is still the same, we loop back to the beginning
        conversationToDisplay.ResetConversation();

        // Take the first dialogue
        currentDialogue = conversationToDisplay.GetNextDialogue();

        // Broadcast that a conversation has begun
        conversationToDisplay.OnConversationBegin.Invoke(); // Invoke the event specific to the conversation
        OnConversationBegin.Invoke();

        // Begin displaying
        StartCoroutine(DetermineLine());
    }

    public void ContinueConversation()
    {
        switch (currentState)
        {
            case State.DisplayingLine:
                {
                    // Make the text display faster
                    waitTime = new WaitForSeconds(characterDisplayWaitTime * characterDisplayWaitTimeMultiplier);
                    break;
                }

            case State.LineEnded:
                {
                    StopAllCoroutines();
                    StartCoroutine(DetermineLine());
                    break;
                }

            case State.ConversationEnded:
                {
                    EndConversation();
                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    public void EndConversation()
    {
        currentState = State.ReadyForConversation;

        // Make sure no coroutines are running
        StopAllCoroutines();

        // Close the display
        currentDisplay?.Display(false);

        // Clear references to avoid null ref errors
        currentDialogue = null;
        currentDisplay = null;
        previousSpeaker = null;

        // Broadcast that the conversation has ended
        conversationToDisplay.OnConversationEnd.Invoke(); // Invoke the event specific to the conversation
        OnConversationEnd.Invoke();
    }

    private IEnumerator DetermineLine()
    {
        currentState = State.DeterminingLine;

        // Start by taking the line to be displayed
        nextLine = currentDialogue.GetNextLine();

        // Check if we're going to need to use another display, i.e. there's another speaker
        if (currentDialogue.speaker != previousSpeaker)
        {
            if (currentDisplay != null) // It will be null at the beginning
                currentDisplay.Display(false); // Hide old display

            previousSpeaker = currentDialogue.speaker;
            currentDisplay = GetDialogueDisplay(currentDialogue.speaker);
        }

        // Do the typewriter effect
        yield return StartCoroutine(DisplayLine());

        // Displaying the line is now done
        currentState = State.LineEnded;
        OnDialogueLineEnd.Invoke();

        if (currentDialogue.HasEnded()) // Check if we're at the end of the current speaker's dialogue
        {
            if (!conversationToDisplay.HasEnded()) // Check if we're not yet at the end of the conversation
                currentDialogue = conversationToDisplay.GetNextDialogue();
            else
                currentState = State.ConversationEnded;
        }
    }

    private Queue<char> textToDisplay = new Queue<char>();

    private IEnumerator DisplayLine()
    {
        textToDisplay.Clear();
        currentDisplay.displayText.text = string.Empty;

        foreach (var character in nextLine)
        {
            textToDisplay.Enqueue(character);
        }

        // Display the line
        currentDisplay?.Display();
        currentState = State.DisplayingLine;

        while (textToDisplay.Count > 0)
        {
            currentDisplay.displayText.text += textToDisplay.Peek();
            textToDisplay.Dequeue();
            yield return waitTime;
        }
    }

    private DialogueDisplay GetDialogueDisplay(DialogueSpeaker dialogueSpeaker)
    {
        if (dialogueDisplays.TryGetValue(dialogueSpeaker, out var dialogueDisplay))
            return dialogueDisplay;

        return null;
    }

    public enum State
    {
        ReadyForConversation,
        DeterminingLine,
        DisplayingLine,
        LineEnded,
        ConversationEnded
    }
}
