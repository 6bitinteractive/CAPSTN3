using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only cares about when the Objective is done
public class ObjectiveStateCondition : Condition
{
    [SerializeField] private GuidReference requiredObjective;

    private Objective reqObjective;

    protected override void InitializeCondition()
    {
        reqObjective = requiredObjective.gameObject.GetComponent<Objective>();
        base.InitializeCondition();

        // If the event has already passed
        if (reqObjective.Complete)
        {
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
        reqObjective.OnDone.RemoveListener(OnObjectiveUpdate);
    }

    protected override bool IsSatisfied()
    {
        return reqObjective.Complete;
    }

    protected override void GetConditionRequirements()
    {
        base.GetConditionRequirements();
        reqObjective.OnDone.AddListener(OnObjectiveUpdate);
    }

    private void OnObjectiveUpdate(Objective objective)
    {
        SwitchStatus(Status.Evaluating);
    }
}
