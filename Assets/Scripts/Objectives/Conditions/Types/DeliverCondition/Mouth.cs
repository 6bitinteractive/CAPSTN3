using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Singleton<Mouth>
{
    private CrossSceneObjectHandler crossSceneDeliverableHandler;
    private EventManager eventManager;

    private GameObject carriedObject;

    private void Start()
    {
        crossSceneDeliverableHandler = SingletonManager.GetInstance<CrossSceneObjectHandler>();
        eventManager = SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<PickupEvent, PickupData>(VerifyPickup);
        eventManager.Subscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
    }

    protected override void OnDestroy()
    {
        eventManager.Unsubscribe<PickupEvent, PickupData>(VerifyPickup);
        eventManager.Unsubscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        base.OnDestroy();
    }

    private void VerifyPickup(PickupData pickupData)
    {
        if (pickupData.type == PickupData.Type.Pickup)
            carriedObject = pickupData.pickupable.gameObject;
        else
            carriedObject = null;
    }

    private void OnEnterArea(EnterArea enterArea)
    {
        if (carriedObject == null) return;
        CrossSceneObject crossSceneObj = carriedObject.GetComponent<CrossSceneObject>();

        if (crossSceneObj == null) return;

        if (crossSceneObj.IsCarried)
        {
            crossSceneDeliverableHandler.carriedObj = carriedObject;
            crossSceneObj.MoveToPersistentScene();
        }
    }

    public void CarryObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<Biteable>() == null)
            Debug.LogErrorFormat("{0} is cannot be carried", gameObject.name);

        carriedObject = gameObject;
    }
}
