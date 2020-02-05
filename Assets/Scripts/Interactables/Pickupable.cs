using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Biteable))]
public class Pickupable : MonoBehaviour
{
    Rigidbody rb;
    Collider collider;

    static EventManager eventManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void Pickup(Interactor source, Transform mouth)
    {
        source.GetComponent<Bite>().IsBiting = true;
        transform.SetParent(mouth.transform);
        transform.localPosition = Vector3.zero;
        collider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;

        eventManager.Trigger<PickupEvent, PickupData>(new PickupData() { source = source, pickupable = this });
    }

    public void DropObject(Interactor source)
    {
        source.GetComponent<Bite>().IsBiting = false;
        transform.SetParent(null);
        collider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
