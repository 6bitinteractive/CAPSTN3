using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentModeCondition : Condition
{
    [SerializeField] private ScentModeData.State requiredState = ScentModeData.State.On;

    private bool conditionSatisfied;

    protected override bool IsSatisfied()
    {
        return conditionSatisfied;
    }

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<ScentModeEvent, ScentModeData>(OnScentMode);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<ScentModeEvent, ScentModeData>(OnScentMode);
    }

    private void OnScentMode(ScentModeData scentModeData)
    {
        conditionSatisfied = scentModeData.state == requiredState;
        SwitchStatus(Status.Evaluating);
    }
}
