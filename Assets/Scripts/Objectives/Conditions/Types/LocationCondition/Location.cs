using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuidComponent))]
[RequireComponent(typeof(BoxCollider))]

public class Location : MonoBehaviour
{
    public Dictionary<GuidComponent, GameObject> objects = new Dictionary<GuidComponent, GameObject>();

    private static EventManager eventManager;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnEnable()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GuidComponent g = other.GetComponent<GuidComponent>();
        if (g != null)
        {
            if (!objects.TryGetValue(g, out GameObject gameObject))
            {
                objects.Add(g, g.gameObject);

                LocationData locationData = new LocationData()
                {
                    location = this,
                    objectInLocation = g.gameObject,
                    type = LocationData.Type.Enter
                };

                eventManager.Trigger<LocationEvent, LocationData>(locationData);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GuidComponent g = other.GetComponent<GuidComponent>();
        if (g != null)
        {
            LocationData locationData = new LocationData()
            {
                location = this,
                objectInLocation = g.gameObject,
                type = LocationData.Type.Exit
            };

            eventManager.Trigger<LocationEvent, LocationData>(locationData);
            objects.Remove(g);
        }
    }
}

