using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

[Serializable]
public class BarkEvent : UnityEvent<Interactor> { }

[RequireComponent(typeof(Collider))]

public class Barkable : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 2f;
    public BarkEvent OnBark;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Interact(Interactor source, IInteractable target)
    {
        //MoveAway(source);
        if (!enabled) return;
        OnBark.Invoke(source);
    }

    public void DisplayInteractability()
    {

    }

    public void MoveAway(Interactor source)
    {
        Vector3 displacement = transform.position - source.transform.position;
        displacement.Normalize(); // Get direction
        displacement *= speed; // speed * direction = new velocity
        displacement += transform.position; // new + old?

        navMeshAgent.SetDestination(displacement);
    }

    public void HideInteractability()
    {

    }
}