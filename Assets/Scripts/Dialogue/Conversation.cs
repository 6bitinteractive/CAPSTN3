using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    public Dialogue[] dialogue;

    [Header("Conversation UnityEvents")]
    public UnityEvent OnConversationBegin = new UnityEvent();
    public UnityEvent OnConversationEnd = new UnityEvent();

    private int currentIndex = -1;

    public Dialogue GetNextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogue.Length)
            return dialogue[currentIndex];

        return null;
    }

    public bool HasEnded()
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
