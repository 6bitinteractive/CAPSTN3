using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlertState : State
{
    // Manually set the target
    [SerializeField] GameObject currentTarget;
    public override void OnEnable()
    {
        base.OnEnable();
        agent.Target = currentTarget;
        navMeshAgent.ResetPath(); // Stop moving

        if (animator == null) return;
        animator.SetBool("IsMoving", true);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (animator == null) return;
        animator.SetBool("IsMoving", false);
    }
}