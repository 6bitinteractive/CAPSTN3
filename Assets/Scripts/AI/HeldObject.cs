using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Pickupable heldObject;
    [SerializeField] private Pickupable originalHeldObject;

    public void OnDisable()
    {
        if (heldObject != null)
            Destroy(heldObject.gameObject);

        SpawnHeldObject();
    }

    public void DropHeldObject()
    {
        if (heldObject == null) return;
        heldObject.DropObject(null);
        heldObject = null;
    }

    public void SpawnHeldObject()
    {
        heldObject = Instantiate(originalHeldObject, hand.transform.position, Quaternion.identity);
        heldObject.transform.parent = hand.transform;
        heldObject.gameObject.SetActive(true);
    }
}
