using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Singleton<Mouth>
{
    private CrossSceneObjectHandler crossSceneDeliverableHandler;
    private GameObject crossSceneGameObjectCarried;
    private CrossSceneObject crossSceneComponent;
    private EventManager eventManager;

    private void Start()
    {
        crossSceneDeliverableHandler = SingletonManager.GetInstance<CrossSceneObjectHandler>();
        eventManager = SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<PickupEvent, PickupData>(CheckDeliverable);
        eventManager.Subscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
    }

    private void OnEnterArea(EnterArea enterArea)
    {
        if (crossSceneComponent == null)
            return;

        if (crossSceneComponent.IsCarried)
            crossSceneComponent.MoveToPersistentScene();
    }

    protected override void OnDestroy()
    {
        eventManager.Unsubscribe<PickupEvent, PickupData>(CheckDeliverable);
        eventManager.Unsubscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        base.OnDestroy();
    }

    private void CheckDeliverable(PickupData pickupData)
    {
        crossSceneComponent = pickupData.pickupable.GetComponent<CrossSceneObject>();

        if (crossSceneComponent == null)
            return;

        crossSceneGameObjectCarried = pickupData.pickupable.gameObject;
        crossSceneDeliverableHandler.carriedObj = crossSceneGameObjectCarried;
    }
}
