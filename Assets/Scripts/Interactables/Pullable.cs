using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Biteable))]
public class Pullable : MonoBehaviour
{
    Rigidbody rb;
    FixedJoint fixedJoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedJoint = GetComponent<FixedJoint>();
    }

    public void Pull(Interactor source)
    {
        source.GetComponent<Bite>().IsBiting = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        fixedJoint.connectedBody = source.GetComponent<Rigidbody>();
    }

    public void StopPulling(Interactor source)
    {
        source.GetComponent<Bite>().IsBiting = false;
        rb.isKinematic = true;
        rb.useGravity = true;
        fixedJoint.connectedBody = null;
    }
}
