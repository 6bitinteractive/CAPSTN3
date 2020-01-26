using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SceneLoader))]

public class EnterArea : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;

    public UnityEvent OnEnterArea = new UnityEvent();

    private BoxCollider boxCollider;
    private bool isLoading;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLoading)
            return;

        StartingPositionHandler positionHandler = other.GetComponent<StartingPositionHandler>();

        if (positionHandler != null)
        {
            int collisionLayerMask = 1 << positionHandler.gameObject.layer;

            // If collides with player display text
            if (collisionLayerMask == playerLayerMask.value)
            {
                isLoading = true;
                OnEnterArea.Invoke();
            }
        }
    }
}
