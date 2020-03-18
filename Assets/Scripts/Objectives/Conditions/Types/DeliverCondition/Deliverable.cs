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

    private void Awake()
    {
        Init();
    }

    public void Activate()
    {
        biteable.enabled = true;
        sniffable.SetCurrentTarget();
    }

    public void OnDeliver()
    {
        if (biteable)
            biteable.enabled = false;

        if (pickupable)
            pickupable.enabled = false;

        if (outlineable)
            outlineable.enabled = false;

        if (rb)
            rb.isKinematic = true;

        sniffable.RemoveCurrentTargetSniffable();
    }

    private void Init()
    {
        biteable = GetComponent<Biteable>();
        pickupable = GetComponent<Pickupable>();
        outlineable = GetComponent<Outlineable>();
        sniffable = GetComponent<Sniffable>();
        rb = GetComponent<Rigidbody>();

        biteable.enabled = false;
    }
}
