using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class Biteable : MonoBehaviour, IInteractable
{
    public UnityEvent OnBite;
    public UnityEvent OnRelease;
    [SerializeField] private Vector3 offset;

    // Cache components since target will always be `this` gameObject; lessen GetComponent<T>() calls
    private Pullable pullableTarget;
    private Pickupable pickupableTarget;

    private void Start()
    {
        pullableTarget = GetComponent<Pullable>();
        pickupableTarget = GetComponent<Pickupable>();
    }

    public void Interact(Interactor source, IInteractable target)
    {
        //Debug.Log("Biting " + gameObject.name);
        if (!enabled) return;
        Bite(source);
    }

    public void DisplayInteractability()
    {

    }

    public void Release(Interactor source)
    {
        if (!source.GetComponent<Bite>().IsBiting) return;

        // Check if Biteable is Pullable
        if (pullableTarget != null)
        {
            pullableTarget.StopPulling(source);
        }
        else if (pickupableTarget != null)
        {
            pickupableTarget.DropObject(source);
        }

        OnRelease.Invoke();
    }

    private void Bite(Interactor source)
    {
        if (source.GetComponent<Bite>().IsBiting) return;

        // Check if Biteable is Pullable
        if (pullableTarget != null && pullableTarget.enabled)
        {
            pullableTarget.Pull(source);
        }

        // Check if Biteable is Pickupable
        else if (pickupableTarget != null && pickupableTarget.enabled)
        {
            Transform mouth = source.GetComponent<Bite>().Mouth.transform;
            Vector3 mouthPos = mouth.localPosition + offset;
            mouth.localPosition = mouthPos;
            pickupableTarget.Pickup(source, mouth);
        }

        OnBite.Invoke();
    }

    public void HideInteractability()
    {

    }
}