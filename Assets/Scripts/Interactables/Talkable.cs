using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, IInteractable
{
    public void DisplayInteractability()
    {
      
    }

    public void Interact(Interactor source, IInteractable target)
    {
        //Call dialogue function here
        Debug.Log(source + "Is talking to " + target);
    }
}