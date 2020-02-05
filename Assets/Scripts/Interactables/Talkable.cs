using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueHandler))]

public class Talkable : MonoBehaviour, IInteractable
{
    public UnityEvent OnTalk;

    private DialogueHandler dialogueHandler;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
    }

    public void DisplayInteractability()
    {

    }

    public void Interact(Interactor source, IInteractable target)
    {
        //Call dialogue function here
        //Debug.Log(source + "Is talking to " + target);
        OnTalk.Invoke();
        dialogueHandler.DisplayCurrentConversation();
    }

    public void HideInteractability()
    {
     
    }
}