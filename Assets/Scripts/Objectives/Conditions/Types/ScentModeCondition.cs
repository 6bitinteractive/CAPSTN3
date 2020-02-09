using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentModeCondition : Condition
{
    private bool scentModeActivated;

    protected override bool IsSatisfied()
    {
        return scentModeActivated;
    }

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<ScentModeEvent, Sniffable>(OnScentMode);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<ScentModeEvent, Sniffable>(OnScentMode);
    }

    private void OnScentMode(Sniffable sniffable)
    {
        scentModeActivated = true;
        SwitchStatus(Status.Evaluating);
    }
}
