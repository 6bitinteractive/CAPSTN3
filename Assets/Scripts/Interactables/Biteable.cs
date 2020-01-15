using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]

public class Biteable : MonoBehaviour, IInteractable
{  
    public Vector3 position =>  transform.position;
    public UnityEvent OnBite;

    public void Interact()
    {
        Debug.Log("Biting " + gameObject.name);
        OnBite.Invoke();
    }

    public void DisplayInteractability()
    {
      
    }
}