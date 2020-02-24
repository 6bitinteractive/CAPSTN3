﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueState : State
{
    [SerializeField] private Vision eyes;
    public override void OnEnable()
    {
        base.OnEnable();
        agent.Target = null;
        navMeshAgent.ResetPath(); // Stop moving
        eyes.gameObject.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        eyes.gameObject.SetActive(true);
    }
}
