using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// NOTE: Getting MissingReference errors? Clear the GuidReference and re-set it again

[RequireComponent(typeof(DialogueHandler))]

public class Requester : MonoBehaviour
{
    [SerializeField] private List<Request> requests = new List<Request>();

    public List<Request> ActiveRequests { get; private set; } = new List<Request>();
    public UnityEvent OnAnyRequestSatisfied;
    public UnityEvent OnAnyRequestUnsatisfied;

    private Dictionary<Condition, Request> requestDict = new Dictionary<Condition, Request>();
    private DeliveryArea deliveryArea;

    private void OnEnable()
    {
        // Listen when an item is dropped around requester
        deliveryArea = GetComponentInChildren<DeliveryArea>();
        deliveryArea.OnDeliverableReceived.AddListener(VerifyDeliverable);

        // Listen to relevant Deliver conditions
        foreach (var request in requests)
        {
            Condition condition = request.deliverCondition.gameObject.GetComponent<Condition>();

            if (condition == null)
                Debug.LogError("Field expects an object with a Condition component.");

            // Only get Conditions that are still relevant
            if (condition.CurrentStatus == Condition.Status.Done)
                continue;

            if (!requestDict.ContainsKey(condition))
            {
                requestDict.Add(condition, request);
                condition.OnActive.gameEvent.AddListener(ActivateRequest);
            }

            // Set the active conditions as the relevant active requests
            if (condition.CurrentStatus == Condition.Status.Active)
                ActivateRequest(condition);
        }
    }

    private void ActivateRequest(Condition condition)
    {
        if (requestDict.TryGetValue(condition, out Request request))
        {
            ActiveRequests.Add(request);
            request.active = true;

            if (request.requestedDeliverableObject.gameObject != null) // If we have the object to deliver in the scene
                request.Deliverable?.Activate();
            else
                Debug.LogErrorFormat("Request deliverable is not found in the same scene.");
        }
    }

    private void OnDisable()
    {
        deliveryArea.OnDeliverableReceived.RemoveListener(VerifyDeliverable);
    }

    public void VerifyDeliverable(Deliverable deliverable)
    {
        if (ActiveRequests == null || ActiveRequests.Count == 0)
            return;

        // Find the related request that has this deliverable
        Request activeRequest = ActiveRequests.Find(x => x.Deliverable == deliverable);

        if (activeRequest != null)
        {
            //Debug.LogFormat("DELIVERED: {0}", requestedItem);

            // Let deliverable do what it wants when it's delivered
            deliverable.OnDeliver();

            // Flag active request as satisfied
            activeRequest.active = false;
            activeRequest.satisfied = true;

            activeRequest.OnRequestSatisfied.Invoke();
            OnAnyRequestSatisfied.Invoke();
        }
        else
        {
            // Find an activeRequest that has not yet been satisfied and show feedback that the request isn't satisfied
            activeRequest = ActiveRequests.Find(x => !x.satisfied);
            activeRequest?.OnRequestUnsatisfied.Invoke();
            OnAnyRequestSatisfied.Invoke();
        }
    }
}

[System.Serializable]
public class Request
{
    public GuidReference deliverCondition;
    public GuidReference requestedDeliverableObject;

    public UnityEvent OnRequestSatisfied;
    public UnityEvent OnRequestUnsatisfied;

    [HideInInspector] public bool active;
    [HideInInspector] public bool satisfied;

    private Deliverable deliverable;
    public Deliverable Deliverable
    {
        get
        {
            deliverable = deliverable ?? requestedDeliverableObject.gameObject.GetComponent<Deliverable>();
            return deliverable;
        }
    }
}
