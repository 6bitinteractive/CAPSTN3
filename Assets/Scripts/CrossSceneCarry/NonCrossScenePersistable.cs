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

        Vector3 newPosition = thisTransform.position;
        switch (resetType)
        {
            case ResetType.OriginalTransform:
                {
                    newPosition = originalPosition;
                    break;
                }

            case ResetType.EnterAreaStartingPoint:
                {
                    EnterArea enterArea = other.GetComponent<EnterArea>();
                    newPosition = enterArea.AssociatedStartingPoint.position;

                    break;
                }
        }

        StartCoroutine(Reposition(newPosition));
    }

    private IEnumerator Reposition(Vector3 position)
    {
        thisCollider.enabled = false; // Disable collision so that it doesn't get moved by another object

        if (rb != null)
            rb.isKinematic = false; // Let if fall on the ground

        while (thisTransform.hasChanged) // Force the position to stick in case it gets repositioned by something else
        {
            thisTransform.position = new Vector3(position.x, thisTransform.position.y, position.z);
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
