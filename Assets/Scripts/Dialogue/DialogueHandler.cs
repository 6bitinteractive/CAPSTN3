﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: Be careful when setting up QuestEvents that are in succession
// Ex. When QuestEvent A is done, it would mean that QuestEventB will start next right away (given current implentation)...
// This means that the dialogue set for QuestEventA for a done status will not be used unless QuestEventB is started
// at a much later time (e.g. manually activating it instead of switching it to active soon after the previous QuestEvent has been done)

public class DialogueHandler : MonoBehaviour
{
    public Conversation CurrentConversation { get; private set; }

    [SerializeField] private Conversation defaultConversation;
    [SerializeField] private List<ConversationSet> questRelatedConversations;

    private static DialogueDisplayManager dialogueDisplayManager;
    private static EventManager eventManager;
    private static Quest quest;

    private void Start()
    {
        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        quest = quest ?? SingletonManager.GetInstance<QuestManager>().CurrentQuest;

        // Listen to quest event status update
        eventManager.Subscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
        //Debug.LogFormat("DialogueHandler for {0} started listening to QuestEventUpdates.", gameObject.name);

        // TODO: Load a saved data

        // Determine what's the current conversation
        CurrentConversation = defaultConversation;
        DetermineCurrentConversation(null);
    }

    private void OnDisable()
    {
        //Debug.LogFormat("DialogueHandler for {0} stopped listening to QuestEventUpdates.", gameObject.name);
        eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
    }

    // This allows you to change the current conversation but not display it right away
    public void SwitchConversation(Conversation conversation)
    {
        CurrentConversation = conversation;
    }

    public void StartConversation()
    {
        // Feed current conversation to DialogueDisplayManager
        if (CurrentConversation != null)
            dialogueDisplayManager.DisplayConversation(CurrentConversation);
    }

    // For times when you want to pass a conversation
    public void StartConversation(Conversation conversation)
    {
        if (conversation != null)
            dialogueDisplayManager.DisplayConversation(conversation);
    }

    private void DetermineCurrentConversation(QuestEvent questEvent)
    {
        if (questRelatedConversations.Count == 0)
        {
            return;
        }

        // Find the related conversation to the quest event
        // If there's no related conversation to the questEvent, we set current conversation to default

        ConversationSet cs = null;
        if (questEvent != null) // Check if it came from a QuestEvent update
        {
            cs = FindRelatedConversation(questEvent);
            if (cs != null)
            {
                CurrentConversation = cs.conversation;
                Debug.LogFormat("Dialogue updated by {0} - {1}", questEvent, questEvent.CurrentStatus);
                Debug.LogFormat("{1} Current Dialogue: {0}", CurrentConversation.dialogue[0].dialogueLines[0], gameObject.name);
                return;
            }
        }

        // Check if theres a conversation related to current quest
        cs = FindRelatedConversation(quest.CurrentQuestEvent);
        if (cs != null)
        {
            CurrentConversation = cs.conversation;
            Debug.LogFormat("{1} Current Dialogue: {0}", CurrentConversation.dialogue[0].dialogueLines[0], gameObject.name);
        }
        else
        {
            cs = FindRelatedConversation(quest.PreviousQuestEvent); // Check if there's a conversation related to the recently done quest

            if (cs != null)
                CurrentConversation = cs.conversation;
            else
                CurrentConversation = defaultConversation;

            Debug.LogFormat("{1} Current Dialogue: {0}", CurrentConversation.dialogue[0].dialogueLines[0], gameObject.name);
        }
    }

    private ConversationSet FindRelatedConversation(QuestEvent questEvent)
    {
        return questRelatedConversations.Find(x => questEvent == x.QuestEvent
                                                && questEvent.CurrentStatus == x.requiredQuestEventStatus);
    }

    [System.Serializable]
    public class ConversationSet
    {
        public GuidReference questEventReference;
        public QuestEvent.Status requiredQuestEventStatus = QuestEvent.Status.Active;
        public Conversation conversation;
        private QuestEvent qe;

        public QuestEvent QuestEvent
        {
            get
            {
                if (qe != null)
                    return qe;
                else
                    qe = questEventReference.gameObject.GetComponent<QuestEvent>();
                return qe;
            }
        }
    }
}
