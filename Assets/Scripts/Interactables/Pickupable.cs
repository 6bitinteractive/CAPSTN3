using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : MonoBehaviour
{
    Rigidbody rb;
    Collider collider;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void Pickup(Interactor source, Transform mouth)
    {
        source.GetComponent<Bite>().IsBiting = true;
        transform.SetParent(mouth.transform);
        transform.localPosition = Vector3.zero;
        collider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
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
