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

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<PickupEvent, PickupData>(GetPickedUpItem);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<PickupEvent, PickupData>(GetPickedUpItem);
    }

    protected override bool IsSatisfied()
    {
        //Debug.LogFormat("PICKED UP: {0} | REQUIRED: {1}", objectPickedUp, objectToBePickedUp);
        // NOTE: For now, it's automatically flagged as Done the moment the object is picked up; it won't care if it was dropped soon after
        return objectToBePickedUp == objectPickedUp;
    }

    private void GetPickedUpItem(PickupData pickupData)
    {
        if (pickupData.type == PickupData.Type.Pickup)
            objectPickedUp = pickupData.pickupable;
        else
            objectPickedUp = null;

        //Debug.Log("Picked up: " + objectPickedUp);

        SwitchStatus(Status.Evaluating);
    }

    protected override void GetConditionRequirements()
    {
        if (objectToGather.gameObject != null) // If we have the scene where the target is located
        {
            //Debug.Log("Getting references for GuidReference");
            objectToBePickedUp = objectToGather.gameObject.GetComponent<Pickupable>();
        }
    }
}
