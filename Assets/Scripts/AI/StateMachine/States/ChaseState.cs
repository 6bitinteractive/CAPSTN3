using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private GameObject sightedIndicator;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float roationSpeed = 10f;
    Attack attack;
    private GameObject currentTarget;
    private float originalSpeed;

    public void Start()
    {
        attack = GetComponent<Attack>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent.ResetPath();
        originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = chaseSpeed;

        if (sightedIndicator)
            sightedIndicator.SetActive(true);

        if (agent.Target != null)
            currentTarget = agent.Target;

        if (animator == null) return;
        animator.SetBool("IsMoving", true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (sightedIndicator)
            sightedIndicator.SetActive(false);

        currentTarget = null;
        navMeshAgent.speed = originalSpeed;

        if (animator == null) return;
        animator.SetBool("IsMoving", false);
    }

    public override void Update()
    {
        base.Update();
        if (currentTarget == null) return;
        RotateTowardsTarget(currentTarget.transform, roationSpeed);
        navMeshAgent.SetDestination(currentTarget.transform.position);
    }
}
