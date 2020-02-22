using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    public Conversation CurrentConversation { get; private set; }

    private static DialogueDisplayManager dialogueDisplayManager;

    private void Start()
    {
        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();
    }

    // This allows you to change the current conversation but not display it right away
    public void SwitchConversation(Conversation conversation)
    {
        CurrentConversation = conversation;
    }

    public void StartConversation()
    {
        // Feed current conversation to DialogueDisplayManager
        if (CurrentConversation != null)
            dialogueDisplayManager.DisplayConversation(CurrentConversation);
    }

    // For times when you want to pass a conversation
    public void StartConversation(Conversation conversation)
    {
        if (conversation != null)
            dialogueDisplayManager.DisplayConversation(conversation);
    }
}
