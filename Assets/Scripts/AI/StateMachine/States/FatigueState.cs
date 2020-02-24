using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueState : State
{
    [SerializeField] private Vision eyes;
    [SerializeField] private AttackRange attackRange;
    public override void OnEnable()
    {
        base.OnEnable();
        agent.Target = null;
        navMeshAgent.ResetPath(); // Stop moving
        eyes.gameObject.SetActive(false);
        attackRange.gameObject.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        eyes.gameObject.SetActive(true);
        attackRange.gameObject.SetActive(true);
    }
}