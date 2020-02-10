using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionState : Condition
{
    [SerializeField] private GuidReference requiredCondition;
    [SerializeField] private Status requiredStatus = Status.Active;

    private Condition reqCondition;
    private Status statusToAssess;

    protected override void InitializeCondition()
    {
        reqCondition = requiredCondition.gameObject.GetComponent<Condition>();
        base.InitializeCondition();

        // If the event has already passed
        if (reqCondition.CurrentStatus != Status.Inactive || reqCondition.CurrentStatus != Status.Evaluating)
        {
            statusToAssess = reqCondition.CurrentStatus;
            SwitchStatus(Status.Evaluating);
        }
        else
        {
            GetConditionRequirements();
        }
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        switch (requiredStatus)
        {
            case Status.Active:
                reqCondition.OnActive.gameEvent.RemoveListener(OnConditionUpdate);
                break;
            case Status.Done:
                reqCondition.OnDone.gameEvent.RemoveListener(OnConditionUpdate);
                break;
        }
    }

    protected override bool IsSatisfied()
    {
        return statusToAssess == requiredStatus;
    }

    protected override void GetConditionRequirements()
    {
        base.GetConditionRequirements();
        switch (requiredStatus)
        {
            case Status.Inactive:
            case Status.Evaluating:
                Debug.LogError("Please don't choose Inactive or Evaluating. Only Active and Done states are supported.");
                break;
            case Status.Active:
                reqCondition.OnActive.gameEvent.AddListener(OnConditionUpdate);
                break;
            case Status.Done:
                reqCondition.OnDone.gameEvent.AddListener(OnConditionUpdate);
                break;
        }
    }

    private void OnConditionUpdate(Condition condition)
    {
        statusToAssess = condition.CurrentStatus;
        SwitchStatus(Status.Evaluating);
    }
}