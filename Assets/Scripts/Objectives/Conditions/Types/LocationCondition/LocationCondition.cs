using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationCondition : Condition
{
    [SerializeField] private GuidReference locationRequired;
    [SerializeField] private GuidReference objectRequiredAtLocation;
    [SerializeField] private LocationData.Type requiredInteraction;

    private LocationData required = new LocationData();
    private LocationData received = new LocationData();

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<LocationEvent, LocationData>(GetLocationData);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<LocationEvent, LocationData>(GetLocationData);
    }

    private void GetLocationData(LocationData locationData)
    {
        received = locationData;
        Debug.Log("REC: " + locationData.location);
        SwitchStatus(Status.Evaluating);
    }

    protected override bool IsSatisfied()
    {
        //Debug.LogFormat("REQUIRED LOCATION: {0} | RECEIVED: {1}\n" +
        //    "REQUIRED OBJECT: {2} | RECEIVED: {3}\n" +
        //    "REQUIRED TYPE: {4} | RECEIVED: {5}",
        //    required.location, received.location,
        //    required.objectInLocation, received.objectInLocation,
        //    required.type, received.type);

        return SameLocationData(required, received);
    }

    public bool SameLocationData(LocationData a, LocationData b)
    {
        return a.location == b.location
            && a.objectInLocation == b.objectInLocation
            && a.type == b.type;
    }

    protected override void GetConditionRequirements()
    {
        if (locationRequired != null)
        {
            required.location = locationRequired.gameObject.GetComponent<Location>();
            required.objectInLocation = objectRequiredAtLocation.gameObject;
            required.type = requiredInteraction;
        }
    }
}
