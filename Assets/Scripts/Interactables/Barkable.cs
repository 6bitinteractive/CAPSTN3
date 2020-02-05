using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]

public class Barkable : MonoBehaviour, IInteractable
{
    public UnityEvent OnBark;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Interact(Interactor source, IInteractable target)
    {
        MoveAway(source);
        OnBark.Invoke();
    }

    public void DisplayInteractability()
    {
       
    }

    private void MoveAway(Interactor source)
    {
        Vector3 directionToSource = transform.position - source.transform.position;
        Vector3 newPos = transform.position + directionToSource;
        navMeshAgent.SetDestination(newPos * 2);
    }

    public void HideInteractability()
    {
        
    }
}