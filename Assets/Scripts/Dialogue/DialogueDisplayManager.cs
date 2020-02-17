﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueDisplayManager : Singleton<DialogueDisplayManager>
{
    [SerializeField] private float defaultCharacterDisplayWaitTime = 0.06f;

    [Tooltip("NOTE: This is a multiplier.\n\nA smaller number means you make the wait time shorter.\n\nShorter wait time means faster display of text.")]
    [SerializeField] private float characterDisplayWaitTimeMultiplier = 0.2f;

    public Dictionary<DialogueSpeaker, DialogueDisplay> dialogueDisplays = new Dictionary<DialogueSpeaker, DialogueDisplay>();

    public UnityEvent OnConversationBegin = new UnityEvent();
    public UnityEvent OnConversationEnd = new UnityEvent();
    public UnityEvent OnDialogueLineEnd = new UnityEvent();

    private Conversation conversationToDisplay;
    private Dialogue currentDialogue;
    private Dialogue previousDialogue = new Dialogue();
    private DialogueSpeaker previousSpeaker;
    private DialogueDisplay currentDisplay;
    private State currentState;
    private string nextLine;
    private float currentCharacterWaitTime;
    private float minCharacterDisplayWaitTime;
    private bool hasTextChanged;

    private void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    private void Start()
    {
        // Fastest (minimum) waiting time
        minCharacterDisplayWaitTime = defaultCharacterDisplayWaitTime * characterDisplayWaitTimeMultiplier * characterDisplayWaitTimeMultiplier;
        ResetDisplayLineSpeed();
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
                    currentCharacterWaitTime *= characterDisplayWaitTimeMultiplier;
                    currentCharacterWaitTime = Mathf.Clamp(currentCharacterWaitTime, minCharacterDisplayWaitTime, defaultCharacterDisplayWaitTime);

                    // If the speed has not reached the minimum, we simply display the full line
                    if (Mathf.Approximately(currentCharacterWaitTime, minCharacterDisplayWaitTime))
                        DisplayFullLine();

                    break;
                }

            case State.LineEnded:
                {
                    ResetDisplayLineSpeed();
                    StopAllCoroutines();

                    StartCoroutine(DetermineLine());
                    break;
                }

            case State.ConversationEnded:
                {
                    EndConversation();
                    break;
                }
        }
    }

    public void EndConversation()
    {
        // Make sure no coroutines are running
        StopAllCoroutines();

        // Close the display
        currentDisplay?.Display(false);

        // Reset everything
        ResetDisplayLineSpeed();

        // Clear references to avoid null ref errors
        currentDialogue = null;
        previousDialogue = null;
        currentDisplay = null;
        previousSpeaker = null;

        // Broadcast that the conversation has ended
        conversationToDisplay.OnConversationEnd.Invoke(); // Invoke the event specific to the conversation
        OnConversationEnd.Invoke();

        // Ready for new conversation
        currentState = State.ReadyForConversation;
    }

    private DialogueDisplay GetDialogueDisplay(DialogueSpeaker dialogueSpeaker)
    {
        if (dialogueDisplays.TryGetValue(dialogueSpeaker, out var dialogueDisplay))
            return dialogueDisplay;

        return null;
    }

    private void ResetDisplayLineSpeed()
    {
        currentCharacterWaitTime = defaultCharacterDisplayWaitTime;
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

        // Check if this is the start of a new dialogue
        if (currentDialogue != previousDialogue)
            currentDialogue.OnDialogueBegin.Invoke(); // Invoke current dialogue's OnDialogueBegin event

        // Do the typewriter effect
        yield return StartCoroutine(DisplayLine(nextLine));

        // Displaying the line is now done
        currentState = State.LineEnded;
        OnDialogueLineEnd.Invoke();

        if (currentDialogue.HasEnded()) // Check if we're at the end of the current speaker's dialogue
        {
            // Invoke current dialogue's OnDialogueEnd event
            // NOTE: This is invoked as soon as the line is done displaying, i.e. no player input is required
            currentDialogue.OnDialogueEnd.Invoke();

            if (!conversationToDisplay.HasEnded()) // Check if we're not yet at the end of the conversation
            {
                previousDialogue = currentDialogue;
                currentDialogue = conversationToDisplay.GetNextDialogue();
            }
            else
            {
                currentState = State.ConversationEnded;
            }
        }
    }

    private void DisplayFullLine()
    {
        totalCharacters = 0;
        currentDisplay.displayText.text = string.Empty;
        currentDisplay.displayText.text = nextLine;
    }

    private int totalCharacters;
    private IEnumerator DisplayLine(string text)
    {
        //currentDisplay.displayText.text = StripAllCommands(text);
        currentDisplay.displayText.text = text;
        currentDisplay.displayText.ForceMeshUpdate();

        //specialCommands = BuildSpecialCommandList(text);

        //Count how many characters we have in our new dialogue line.
        TMP_TextInfo textInfo = currentDisplay.displayText.textInfo;
        totalCharacters = currentDisplay.displayText.textInfo.characterCount;

        //Color of all characters' vertices.
        Color32[] newVertexColors;

        //Base color for our text.
        Color32 c0 = currentDisplay.displayText.color;

        // Display the line
        currentDisplay?.Display();
        currentState = State.DisplayingLine;

        //Shake text if true.
        //if (isTextShaking)
        //{
        //    StartCoroutine(ShakingText());
        //}

        //We now hide text based on each character's alpha value
        HideText();

        int i = 0;
        while (i < totalCharacters)
        {

            //If we change the text live on runtime in our inspector, adjust the character count!
            if (hasTextChanged)
            {
                totalCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false;
            }

            /*  Note: implementing a color command is easy now! All you need to do is
             *  extract the value, create a bool isColorizing = true, and use this color instead
             *  of the base c0 color. A second command can put isColorizing to false.
             *  I leave it up to you to figure this out.
            */
            //if (specialCommands.Count > 0)
            //{
            //    CheckForCommands(i);
            //}

            //Instead of incrementing maxVisibleCharacters or add the current character to our string, we do this :

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            // Only change the vertex color if the text element is visible. (It's visible, only the alpha color is 0)
            if (textInfo.characterInfo[i].isVisible)
            {

                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c0;

                // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                currentDisplay.displayText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }

            i++;
            yield return new WaitForSeconds(currentCharacterWaitTime);
        }
    }

    //Hide our text by making all our characters invisible.
    private void HideText()
    {
        currentDisplay.displayText.ForceMeshUpdate();

        TMP_TextInfo textInfo = currentDisplay.displayText.textInfo;

        Color32[] newVertexColors;
        Color32 c0 = currentDisplay.displayText.color;

        for (int i = 0; i < textInfo.characterCount; i++)
        {

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            //Alpha = 0
            c0 = new Color32(c0.r, c0.g, c0.b, 0);

            //Apply it to all vertex.
            newVertexColors[vertexIndex + 0] = c0;
            newVertexColors[vertexIndex + 1] = c0;
            newVertexColors[vertexIndex + 2] = c0;
            newVertexColors[vertexIndex + 3] = c0;

            // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
            currentDisplay.displayText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    private void OnTextChanged(UnityEngine.Object obj)
    {
        hasTextChanged = true;
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

// Source - typewriter effect:
// https://bitbucket.org/flaredust/excerpts-of-video-game-code-for-unity/src/master/project-libra/RPG%20Dialogue%20System/DialogueExample5.cs
