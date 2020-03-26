using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class DialogueDisplayManager : Singleton<DialogueDisplayManager>
{
    [SerializeField] private float defaultCharacterDisplayWaitTime = 0.06f;

    [Tooltip("NOTE: This is a multiplier.\n\nA smaller number means you make the wait time shorter.\n\nShorter wait time means faster display of text.")]
    [SerializeField] private float characterDisplayWaitTimeMultiplier = 0.2f;

    [SerializeField] private AudioClip TextScrollSFX;

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
    private float widthPadding = 100f;
    private AudioSource audioSource;
    private SpecialCommandHandler specialCommandHandler;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        specialCommandHandler = new SpecialCommandHandler();

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

                    // If the speed has reached the minimum (i.e. the fastest), we then display the full line
                    // We infer that the player must want to see the full line ASAP
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
        if (conversationToDisplay == null)
            return;

        // Make sure no coroutines are running
        StopAllCoroutines();

        // Close the display
        currentDisplay?.Display(false);

        // Reset some references
        ResetDisplayLineSpeed();
        currentDialogue = null;
        previousDialogue = null;
        currentDisplay = null;
        previousSpeaker = null;

        // Broadcast that the conversation has ended
        conversationToDisplay.OnConversationEnd.Invoke(); // Invoke the event specific to the conversation
        OnConversationEnd.Invoke();

        // Reset the SpecialCommandHandler
        specialCommandHandler.ResetSpecialCommandList();

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

        // Play text scroll SFX
        audioSource.clip = TextScrollSFX;
        audioSource.Play();

        // Check if we're going to need to use another display, i.e. there's another speaker
        if (currentDialogue.speaker != previousSpeaker)
        {
            if (currentDisplay != null) // It will be null at the beginning
                currentDisplay.Display(false); // Hide old display

            previousSpeaker = currentDialogue.speaker;
            currentDisplay = GetDialogueDisplay(currentDialogue.speaker);
        }

        // Hide the button prompt
        currentDisplay?.ShowButton(false);

        // Check if this is the start of a new dialogue
        if (currentDialogue != previousDialogue)
            currentDialogue.OnDialogueBegin.Invoke(); // Invoke current dialogue's OnDialogueBegin event

        // Do the typewriter effect
        //yield return StartCoroutine(SimpleDisplayLine());
        yield return StartCoroutine(DisplayLine(new string(nextLine.ToCharArray())));

        // Displaying the line is now done
        currentState = State.LineEnded;
        currentDisplay.ShowButton(true);
        audioSource.Stop();
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
        // For typewriter effect using TMP
        totalCharacters = 0;
        currentDisplay.displayText.text = string.Empty;
        currentDisplay.displayText.maxVisibleCharacters = nextLine.Length;
        Vector2 preferred = currentDisplay.displayText.GetPreferredValues(nextLine); // Calculate the dimension of the text to be displayed
        currentDisplay.layoutElement.preferredWidth = preferred.x + widthPadding; // Set the width

        // For simple typewriter effect
        textToDisplay.Clear();

        currentDisplay.displayText.text = nextLine;
    }

    #region Basic typewritter effect
    private Queue<char> textToDisplay = new Queue<char>();

    private IEnumerator SimpleDisplayLine()
    {
        textToDisplay.Clear();
        currentDisplay.displayText.text = string.Empty;

        foreach (var character in nextLine)
            textToDisplay.Enqueue(character);

        // Display the line
        currentDisplay?.Display();
        currentState = State.DisplayingLine;

        while (textToDisplay.Count > 0)
        {
            currentDisplay.displayText.text += textToDisplay.Peek();
            textToDisplay.Dequeue();
            yield return new WaitForSeconds(currentCharacterWaitTime);
        }
    }
    #endregion

    #region Typewriter effect using TextMeshPro
    // This implementation fixes word-wrapping issues for very long texts that span multiple lines

    private void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    private bool hasTextChanged;
    private int totalCharacters;

    private IEnumerator DisplayLine(string text)
    {
        // Format the text (change size, color, styling if there are any keywords)
        currentDisplay.displayText.text = specialCommandHandler.FormatText(text);
        currentDisplay.displayText.ForceMeshUpdate();

        // Build the command list
        specialCommandHandler.BuildCommandList(currentDisplay.displayText.text);

        // Clean up the text of special commands
        currentDisplay.displayText.text = specialCommandHandler.StripAllCommands(currentDisplay.displayText.text);

        // Count how many characters we have in our new dialogue line.
        TMP_TextInfo textInfo = currentDisplay.displayText.textInfo;
        totalCharacters = currentDisplay.displayText.textInfo.characterCount;

        // Color of all characters' vertices.
        Color32[] newVertexColors;

        //Base color for our text.
        Color32 c0 = currentDisplay.displayText.color;

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

            // Execute special commands if there are any
            if (specialCommandHandler.SpecialCommands.Count > 0)
                specialCommandHandler.ExecuteCommand(i);

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

            // Increment
            i++;

            // Display i-number of characters
            currentDisplay.displayText.maxVisibleCharacters = i;
            currentDisplay.displayText?.ForceMeshUpdate(); // Update to avoid flicker

            // Force width of container to only be as big as the visible text
            Vector2 rendered = currentDisplay.displayText.GetRenderedValues();
            if (currentDisplay.layoutElement != null) // We check for null in cases where dialogue is in the middle of displaying while entering scene transition
                currentDisplay.layoutElement.preferredWidth = rendered.x + widthPadding;

            // Display the line
            currentDisplay?.Display();
            currentState = State.DisplayingLine;

            // Wait to give that typewriter effect
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
    #endregion

    public enum State
    {
        ReadyForConversation,
        DeterminingLine,
        DisplayingLine,
        LineEnded,
        ConversationEnded
    }
}

// Source - typewriter effect using TextMeshPro:
// https://bitbucket.org/flaredust/excerpts-of-video-game-code-for-unity/src/master/project-libra/RPG%20Dialogue%20System/DialogueExample5.cs
