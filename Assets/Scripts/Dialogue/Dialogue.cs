using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public DialogueSpeaker speaker;
    public string[] dialogueLines;

    private int currentIndex = -1;

    public string GetNextLine()
    {
        currentIndex++;
        if (currentIndex < dialogueLines.Length)
        {
            return dialogueLines[currentIndex];
        }
        else
        {
            return null;
        }
    }

    // Check if the dialogue will be ending
    public bool HasEnded()
    {
        return currentIndex + 1 >= dialogueLines.Length;
    }

    public void ResetDialogue()
    {
        currentIndex = -1;
    }
}
