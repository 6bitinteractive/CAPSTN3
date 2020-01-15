using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]

public class Barkable : MonoBehaviour, IInteractable
{
    public Vector3 position => transform.position;
    public UnityEvent OnBark;

    public void Interact()
    {
        Debug.Log("Woof");
        OnBark.Invoke();
    }

    public void DisplayInteractability()
    {
       
    }
}