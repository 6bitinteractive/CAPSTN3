using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SceneLoader))]

public class EnterArea : MonoBehaviour
{
    [SerializeField] Transform associatedStartingPoint;
    [SerializeField] private LayerMask playerLayerMask;

    public Transform AssociatedStartingPoint => associatedStartingPoint;
    public UnityEvent OnEnterArea = new UnityEvent();

    private BoxCollider boxCollider;
    private bool isLoading;

    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
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

            if (collisionLayerMask == playerLayerMask.value)
            {
                isLoading = true;
                OnEnterArea.Invoke();
                eventManager.Trigger<EnterAreaEvent, EnterArea>(this);
            }
        }
    }
}
