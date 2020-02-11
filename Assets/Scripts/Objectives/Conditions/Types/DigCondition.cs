using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigCondition : Condition
{
    private bool foundDigable;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<DigableEvent, Digable>(Evaluate);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<DigableEvent, Digable>(Evaluate);
    }

    protected override bool IsSatisfied()
    {
        return foundDigable;
    }

    private void Evaluate(Digable digable)
    {
        // This assumes that the digable spot that spawned the object is the correct spot
        foundDigable = true;
        SwitchStatus(Status.Evaluating);
    }
}
