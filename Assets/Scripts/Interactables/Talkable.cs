using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Talkable : MonoBehaviour, IInteractable
{
    public UnityEvent OnTalk;
    public void DisplayInteractability()
    {
      
    }

    public void Interact(Interactor source, IInteractable target)
    {
        //Call dialogue function here
        Debug.Log(source + "Is talking to " + target);
        OnTalk.Invoke();
    }
}