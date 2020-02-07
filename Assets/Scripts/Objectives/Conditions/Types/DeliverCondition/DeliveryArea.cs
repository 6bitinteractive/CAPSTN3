using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class DeliveryArea : MonoBehaviour
{
    public DeliveryEvent OnDeliverableReceived = new DeliveryEvent();
    private BoxCollider boxCollider;
    private Deliverable deliverable;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Deliverable newDeliverable = other.GetComponent<Deliverable>();
        if (newDeliverable != null && newDeliverable != deliverable)
        {
            deliverable = newDeliverable;
            OnDeliverableReceived.Invoke(deliverable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        deliverable = null;
    }
}
