using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneDeliverableController : MonoBehaviour
{
    [SerializeField] private GuidReference deliverableObject;

    private Deliverable Deliverable => deliverableObject.gameObject.GetComponent<Deliverable>();

    // We only get references when the method is called

    public void SetActive(bool value)
    {
        if (deliverableObject.gameObject == null) return;
        deliverableObject.gameObject.SetActive(value);
    }

    public void Enable(bool updateData)
    {
        if (deliverableObject.gameObject == null) return;
        Deliverable.Enable(updateData);
    }

    public void Disable(bool updateData)
    {
        if (deliverableObject.gameObject == null) return;
        Deliverable.Disable();
    }

    public void SetParent(Transform transform)
    {
        if (deliverableObject.gameObject == null) return;
        deliverableObject.gameObject.transform.SetParent(transform);
    }

    public void IsKinematic(bool value)
    {
        if (deliverableObject.gameObject == null) return;
        deliverableObject.gameObject.GetComponent<Rigidbody>().isKinematic = value;
    }

    public void Activate()
    {
        if (deliverableObject.gameObject == null) return;
        Deliverable.Activate();
    }
}
