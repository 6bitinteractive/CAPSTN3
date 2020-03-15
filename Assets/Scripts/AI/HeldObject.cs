using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private Pickupable heldObject;
    [SerializeField] private Pickupable originalHeldObject;

    private Vector3 originalScale;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Transform heldObjectTransform;
    private void Start()
    {
        originalScale = originalHeldObject.transform.localScale;
        originalPos = originalHeldObject.transform.localPosition;
        originalRotation = originalHeldObject.transform.localRotation;
    }

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
        ResetLocalTransform(heldObject);

        heldObject.gameObject.SetActive(true);
    }

    private void ResetLocalTransform(Pickupable heldObject)
    {
        heldObjectTransform = heldObject.transform;
        heldObjectTransform.parent = hand.transform;
        heldObjectTransform.localScale = originalScale;
        heldObjectTransform.localPosition = originalPos;
        heldObjectTransform.localRotation = originalRotation;
    }
}