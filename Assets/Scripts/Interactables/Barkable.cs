using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]

public class Barkable : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 2f;
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