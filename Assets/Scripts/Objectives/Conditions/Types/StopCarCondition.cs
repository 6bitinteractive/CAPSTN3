using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCarCondition : Condition
{
    private static string carObjectName = "Car";
    private string objectName;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<SwitchStateEvent, State>(VerifyAgent);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<SwitchStateEvent, State>(VerifyAgent);
    }

    protected override bool IsSatisfied()
    {
        return objectName.StartsWith(carObjectName);
    }

    private void VerifyAgent(State state)
    {
        //Debug.Log(state);
        if (state is IdleState)
            objectName = state.gameObject.name;
        else
            objectName = string.Empty;

        SwitchStatus(Status.Evaluating);
    }
}
