using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Singleton<Mouth>
{
    private CrossSceneDeliverableHandler crossSceneDeliverableHandler;
    private GameObject deliverableCarried;
    private Deliverable deliverable;
    private EventManager eventManager;

    private void Start()
    {
        crossSceneDeliverableHandler = SingletonManager.GetInstance<CrossSceneDeliverableHandler>();
        eventManager = SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<PickupEvent, PickupData>(CheckDeliverable);
        eventManager.Subscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
    }

    private void OnEnterArea(EnterArea enterArea)
    {
        if (deliverable == null)
            return;

        if (deliverable.IsCarried)
            deliverable.MoveToPersistent();
    }

    protected override void OnDestroy()
    {
        eventManager.Unsubscribe<PickupEvent, PickupData>(CheckDeliverable);
        eventManager.Unsubscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        base.OnDestroy();
    }

    private void CheckDeliverable(PickupData pickupData)
    {
        deliverable = pickupData.pickupable.GetComponent<Deliverable>();

        if (deliverable == null)
            return;

        // We only care about cross-scene deliverables for now
        if (!deliverable.IsCrossSceneDeliverable)
            return;

        deliverableCarried = pickupData.pickupable.gameObject;
        crossSceneDeliverableHandler.DeliverableObj = deliverableCarried;
    }
}
