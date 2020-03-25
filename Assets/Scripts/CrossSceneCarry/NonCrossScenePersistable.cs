using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NonCrossScenePersistable : MonoBehaviour
{
    [SerializeField] private LayerMask enterAreaLayerMask;

    [Tooltip("Reset transform to which value")]
    [SerializeField] private ResetType resetType;

    public UnityEvent OnRepositionDone;

    private Rigidbody rb;
    private Collider thisCollider;
    private Transform thisTransform;
    private Vector3 originalPosition;

    private void Awake()
    {
        thisTransform = gameObject.transform;
        originalPosition = thisTransform.position;

        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int collisionLayerMask = 1 << other.gameObject.layer;

        if (collisionLayerMask != enterAreaLayerMask.value) return;

        switch (resetType)
        {
            case ResetType.OriginalTransform:
                {
                    thisTransform.position = originalPosition;
                    break;
                }

            case ResetType.EnterAreaStartingPoint:
                {

                    StartCoroutine(Reposition(other));

                    break;
                }
        }
    }

    private IEnumerator Reposition(Collider other)
    {
        thisCollider.enabled = false; // Disable collision so that it doesn't get moved by another object

        EnterArea enterArea = other.GetComponent<EnterArea>();
        Vector3 posStartingPoint = enterArea.AssociatedStartingPoint.position;

        if (rb != null)
            rb.isKinematic = true; // Let if fall on the ground

        while (thisTransform.hasChanged) // Force the position to stick in case it gets repositioned by something else
        {
            thisTransform.position = new Vector3(posStartingPoint.x, thisTransform.position.y, posStartingPoint.z);
            yield return null;
        }

        OnRepositionDone.Invoke();
    }

    public enum ResetType
    {
        OriginalTransform,
        EnterAreaStartingPoint
    }
}
