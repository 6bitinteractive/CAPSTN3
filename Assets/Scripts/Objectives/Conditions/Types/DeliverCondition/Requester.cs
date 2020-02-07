using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueHandler))]

public class Requester : MonoBehaviour
{
    [SerializeField] private List<Request> requests = new List<Request>();

    public UnityEvent OnRequestSatisfied = new UnityEvent();
    public Request CurrentRequest { get; private set; }

    private Dictionary<QuestEvent, Request> requestDict = new Dictionary<QuestEvent, Request>();
    private QuestEvent currentQuestEvent;
    private DeliveryArea deliveryArea;
    private DialogueHandler dialogueHandler;

    private void OnEnable()
    {
        dialogueHandler = GetComponent<DialogueHandler>();

        // Listen when an item is dropped around requester
        deliveryArea = GetComponentInChildren<DeliveryArea>();
        deliveryArea.OnDeliverableReceived.AddListener(VerifyDeliverable);

        // Listen to relevant QuestEvents
        foreach (var request in requests)
        {
            QuestEvent questEvent = request.questEvent.gameObject.GetComponent<QuestEvent>();

            // Only get questEvents that are still relevant
            if (questEvent.CurrentStatus == QuestEvent.Status.Done)
                continue;

            requestDict.Add(questEvent, request);
            questEvent.OnActive.gameEvent.AddListener(ActivateRequest);

            // Set the active quest event as the currently relevant questEvent
            if (questEvent.CurrentStatus == QuestEvent.Status.Active)
                ActivateRequest(questEvent);
        }
    }

    private void ActivateRequest(QuestEvent questEvent)
    {
        if (requestDict.TryGetValue(questEvent, out Request request))
        {
            currentQuestEvent = questEvent;
            CurrentRequest = request;
            CurrentRequest.active = true;
        }
    }

    private void OnDisable()
    {
        deliveryArea.OnDeliverableReceived.RemoveListener(VerifyDeliverable);
    }

    public void VerifyDeliverable(Deliverable deliverable)
    {
        if (CurrentRequest != null)
        {
            Deliverable requestedItem = CurrentRequest.requestedObject.gameObject.GetComponent<Deliverable>();
            if (deliverable == requestedItem)
            {
                Debug.Log("DELIVERED");
                CurrentRequest.active = false;
                CurrentRequest.satisfied = true;
                dialogueHandler.StartConversation(CurrentRequest.conversationSatisfied);
                OnRequestSatisfied.Invoke();

                // Clear references
                CurrentRequest = null;
                currentQuestEvent = null;
            }
            else
            {
                dialogueHandler.StartConversation(CurrentRequest.conversationUnsatisfied);
            }
        }
    }
}

[System.Serializable]
public class Request
{
    public GuidReference questEvent;
    public GuidReference requestedObject;
    public Conversation conversationSatisfied;
    public Conversation conversationUnsatisfied;
    [HideInInspector] public bool active;
    [HideInInspector] public bool satisfied;
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
