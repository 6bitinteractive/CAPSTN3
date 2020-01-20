using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Biteable : MonoBehaviour, IInteractable
{  
    public UnityEvent OnBite;
    public UnityEvent OnRelease;

    public void Interact(Interactor source, IInteractable target)
    {
        //Debug.Log("Biting " + gameObject.name);
        Bite(source);
    }
    public void DisplayInteractability()
    {
      
    }

    public void Release(Interactor source)
    {
        if (!source.GetComponent<Bite>().IsBiting) return;

        // Check if Biteable is Pullable
        if (gameObject.GetComponent<Pullable>() != null)
        {
            Pullable pullableTarget = gameObject.GetComponent<Pullable>();
            pullableTarget.StopPulling(source);
        }

        else if (gameObject.GetComponent<Pickupable>() != null)
        {
            Pickupable pickupableTarget = gameObject.GetComponent<Pickupable>();
            pickupableTarget.DropObject(source);
        }

        OnRelease.Invoke();
    }

    private void Bite(Interactor source)
    {
        if (source.GetComponent<Bite>().IsBiting) return;

        // Check if Biteable is Pullable
        if (gameObject.GetComponent<Pullable>() != null)
        {
            Pullable pullableTarget = gameObject.GetComponent<Pullable>();
            pullableTarget.Pull(source);
        }

        // Check if Biteable is Pickupable
        else if (gameObject.GetComponent<Pickupable>() != null)
        {
            Pickupable pickupableTarget = gameObject.GetComponent<Pickupable>();
            Transform mouth = source.GetComponent<Bite>().Mouth.transform;
            pickupableTarget.Pickup(source, mouth);
        }

        OnBite.Invoke();
    }
}