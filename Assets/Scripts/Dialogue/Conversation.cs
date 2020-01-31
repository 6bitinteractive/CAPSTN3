using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation
{
    public Dialogue[] dialogue;

    private int currentIndex = -1;

    public Dialogue GetNextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogue.Length)
        {
            return dialogue[currentIndex];
        }
        else
        {
            currentIndex = -1;
            return null;
        }
    }

    public bool NextIsEnd()
    {
        return currentIndex + 1 >= dialogue.Length;
    }

    public void ResetConversation()
    {
        currentIndex = -1;

        foreach (var d in dialogue)
        {
            d.ResetDialogue();
        }
    }
}
