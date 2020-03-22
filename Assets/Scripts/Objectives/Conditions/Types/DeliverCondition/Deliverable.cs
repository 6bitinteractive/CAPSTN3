using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sniffable))]

public class Deliverable : MonoBehaviour
{
    private Biteable biteable;
    private Pickupable pickupable;
    private Outlineable outlineable;
    private Sniffable sniffable;
    private Rigidbody rb;
    private Collider collider;

    private void Awake()
    {
        Init();
    }

    public void Activate()
    {
        Init();
        collider.enabled = true;
        biteable.enabled = true;
        outlineable.enabled = true;
        sniffable.SetCurrentTarget();
    }

    public void OnDeliver()
    {
        if (biteable)
            biteable.enabled = false;

        if (pickupable)
            pickupable.enabled = false;

        if (outlineable)
        {
           outlineable.HideInteractability();
           outlineable.enabled = false;           
        }

        if (rb)
            rb.isKinematic = true;

        sniffable.RemoveCurrentTargetSniffable();
    }

    private void Init()
    {
        biteable = biteable ?? GetComponent<Biteable>();
        pickupable = pickupable ?? GetComponent<Pickupable>();
        outlineable = outlineable ?? GetComponent<Outlineable>();
        sniffable = sniffable ?? GetComponent<Sniffable>();
        rb = rb ?? GetComponent<Rigidbody>();
        collider = collider ?? GetComponent<Collider>();

        collider.enabled = false;
        biteable.enabled = false;
        outlineable.enabled = false;
    }
}
