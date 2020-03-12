using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliverable : MonoBehaviour
{
    private Biteable biteable;
    private Pickupable pickupable;
    private Outlineable outlineable;
    private Rigidbody rb;

    private void Awake()
    {
        biteable = GetComponent<Biteable>();
        pickupable = GetComponent<Pickupable>();
        outlineable = GetComponent<Outlineable>();
        rb = GetComponent<Rigidbody>();
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
    }
}
