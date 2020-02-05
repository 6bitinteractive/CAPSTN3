using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatherCondition : Condition
{
    [Header("Condition Requirements")]
    [SerializeField] private GuidReference objectToGather;
    private Pickupable objectToBePickedUp;
    private Pickupable objectPickedUp;

    protected override bool RequireSceneLoad => true;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<PickupEvent, PickupData>(GetPickedUpItem);
        GetConditionRequirements();
    }

    protected override void EvaluateCondition()
    {
        base.EvaluateCondition();
        Debug.LogFormat("PICKED UP: {0} | REQUIRED: {1}", objectPickedUp, objectToBePickedUp);
        if (objectToBePickedUp == objectPickedUp)
            SwitchStatus(Status.Done);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<PickupEvent, PickupData>(GetPickedUpItem);
    }

    private void GetPickedUpItem(PickupData pickupData)
    {
        objectPickedUp = pickupData.pickupable;
        SwitchStatus(Status.Evaluating);
        Debug.Log("Picked up: " + objectPickedUp);
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        GetConditionRequirements();
    }

    private void GetConditionRequirements()
    {
        if (objectToGather.gameObject != null) // If we have the scene where the target is located
        {
            //Debug.Log("Getting references for GuidReference");
            objectToBePickedUp = objectToGather.gameObject.GetComponent<Pickupable>();
        }
    }
}
