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
    static EventManager eventManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedJoint = GetComponent<FixedJoint>();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void Pull(Interactor source)
    {
        source.GetComponent<Bite>().IsBiting = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        fixedJoint.connectedBody = source.GetComponent<Rigidbody>();

        PullData pullData = new PullData()
        {
            source = source,
            pullable = this,
            type = PullData.Type.Pull
        };
        eventManager.Trigger<PullEvent, PullData>(pullData);
    }

    public void StopPulling(Interactor source)
    {
        source.GetComponent<Bite>().IsBiting = false;
        rb.isKinematic = true;
        rb.useGravity = true;
        fixedJoint.connectedBody = null;

        PullData pullData = new PullData()
        {
            source = source,
            pullable = this,
            type = PullData.Type.Release
        };
        eventManager.Trigger<PullEvent, PullData>(pullData);
    }
}
