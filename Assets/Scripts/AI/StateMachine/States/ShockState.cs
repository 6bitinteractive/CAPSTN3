using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockState : State
{
    [SerializeField] private GameObject emoticon;

    public override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent.ResetPath(); // Stop moving

        if (emoticon)
            emoticon.SetActive(true);

        if (animator == null) return;
        animator.SetTrigger("Shock");
    }

    public override void OnDisable()
    {
        base.OnDisable();
        agent.TargetWithinRange = null;
        if (emoticon)
            emoticon.SetActive(false);
    }

    public override void Update()
    {
        if (agent.TargetWithinRange == null) return;
        base.Update();
        RotateTowardsTarget(agent.TargetWithinRange.transform, 2);
    }
}