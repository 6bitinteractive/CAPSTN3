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

    public UnityEvent OnRequestSatisfied = new UnityEvent();
    public List<Request> ActiveRequests { get; private set; } = new List<Request>();

    private Dictionary<Condition, Request> requestDict = new Dictionary<Condition, Request>();
    private DeliveryArea deliveryArea;
    private DialogueHandler dialogueHandler;

    private void OnEnable()
    {
        dialogueHandler = GetComponent<DialogueHandler>();

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
            request.Deliverable?.Activate();
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

            if (activeRequest.conversationSatisfied != null)
                dialogueHandler.StartConversation(activeRequest.conversationSatisfied);

            OnRequestSatisfied.Invoke();
        }
        else
        {
            // Find an activeRequest that has not yet been satisfied and show dialogue feedback that the request isn't satisfied
            // This just shows one dialogue among all possible feedback, not necessarily the particular request the player is trying to satisfy
            activeRequest = ActiveRequests.Find(x => !x.satisfied);
            if (activeRequest?.conversationUnsatisfied != null)
                dialogueHandler.StartConversation(activeRequest.conversationUnsatisfied);
        }
    }
}

[System.Serializable]
public class Request
{
    public GuidReference deliverCondition;
    public GuidReference requestedDeliverableObject;
    public Conversation conversationSatisfied;
    public Conversation conversationUnsatisfied;
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

/*
 * Player gathers an item
 * Player drops item to delivery area
 * Delivery area alerts requester that there was an item delivered
 * Requester checks if delivery is correct
 * Requester sets a new conversation depending on whether the item satisfies the request or not
 * Requester gets dialogueHandler to Start a conversation
 * Request is flagged satisfied
 */
