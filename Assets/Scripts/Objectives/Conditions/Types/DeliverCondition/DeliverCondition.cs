using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// NOTE: The particulars of the request are defined by the Requester

public class DeliverCondition : Condition
{
    [SerializeField] private GuidReference requesterGuid;
    private Requester requester;
    private bool requestSatisfied;

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        requester.OnRequestSatisfied.RemoveListener(Evaluate);
    }

    protected override bool IsSatisfied()
    {
        return requestSatisfied;
    }

    private void Evaluate()
    {
        requestSatisfied = requester.ActiveRequests.Exists(x => x.deliverCondition.gameObject.GetComponent<Condition>() == this && x.satisfied);
        SwitchStatus(Status.Evaluating);
    }

    protected override void GetConditionRequirements()
    {
        if (requesterGuid.gameObject != null) // If we have the scene where the target is located
        {
            //Debug.Log("Getting references for GuidReference");
            requester = requesterGuid.gameObject.GetComponent<Requester>();
            requester.OnRequestSatisfied.AddListener(Evaluate);
        }
    }
}
