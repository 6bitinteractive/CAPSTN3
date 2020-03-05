using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] string animationToPlayName;

    public override void OnEnable()
    {
        base.OnEnable();
        if (animator == null) return;
        animator.SetTrigger(animationToPlayName);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (animator == null) return;
        animator.ResetTrigger(animationToPlayName);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        if (agent.TargetWithinRange == null) return;     
    }
}