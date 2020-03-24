using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneDeliverableController : MonoBehaviour
{
    [SerializeField] private GuidReference deliverableObject;

    private Deliverable deliverable;

    // We only get references when the method is called
    public void Enable()
    {
        if (deliverableObject.gameObject == null) return;
        deliverable = deliverableObject.gameObject.GetComponent<Deliverable>();
        deliverable.Enable();
    }

    public void Disable()
    {
        if (deliverableObject.gameObject == null) return;
        deliverable = deliverableObject.gameObject.GetComponent<Deliverable>();
        deliverable.Disable();
    }
}
