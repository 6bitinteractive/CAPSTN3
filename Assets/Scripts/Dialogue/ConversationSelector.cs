using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: Be careful when setting up QuestEvents that are in succession
// Ex. When QuestEvent A is done, it would mean that QuestEventB will start next right away (given current implentation)...
// This means that the dialogue set for QuestEventA for a done status will not be used unless QuestEventB is started
// at a much later time (e.g. manually activating it instead of switching it to active soon after the previous QuestEvent has been done)

[RequireComponent(typeof(DialogueHandler))]

public class ConversationSelector : MonoBehaviour
{
    [SerializeField] private Conversation defaultConversation;
    [SerializeField] private List<ConversationSet> questRelatedConversations;

    private Conversation currentConversation;
    private DialogueHandler dialogueHandler;
    private static EventManager eventManager;
    private static Quest quest;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        quest = quest ?? SingletonManager.GetInstance<QuestManager>().CurrentQuest;

        // Listen to quest event status update
        eventManager.Subscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
        //Debug.LogFormat("DialogueHandler for {0} started listening to QuestEventUpdates.", gameObject.name);

        // TODO: Load a saved data

        // Start with the default conversation
        currentConversation = defaultConversation;
        dialogueHandler.SwitchConversation(currentConversation);

        // Determine what's the current conversation
        DetermineCurrentConversation(null);
    }

    private void OnDisable()
    {
        //Debug.LogFormat("DialogueHandler for {0} stopped listening to QuestEventUpdates.", gameObject.name);
        eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
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
                currentConversation = cs.conversation;
                dialogueHandler.SwitchConversation(currentConversation);

                //Debug.LogFormat("Dialogue updated by {0} - {1}", questEvent, questEvent.CurrentStatus);
                // PrintConversation();
                return;
            }
        }

        // Check if theres a conversation related to current quest
        cs = FindRelatedConversation(quest.CurrentQuestEvent);
        if (cs != null)
        {
            currentConversation = cs.conversation;
        }
        else
        {
            cs = FindRelatedConversation(quest.PreviousQuestEvent); // Check if there's a conversation related to the recently done quest

            if (cs != null)
                currentConversation = cs.conversation;
            else
                currentConversation = defaultConversation;

        }

        dialogueHandler.SwitchConversation(currentConversation);

        // PrintConversation();
    }

    private ConversationSet FindRelatedConversation(QuestEvent questEvent)
    {
        return questRelatedConversations.Find(x => questEvent == x.QuestEvent
                                                && questEvent.CurrentStatus == x.requiredQuestEventStatus);
    }

    private void PrintConversation()
    {
        if (currentConversation != null)
            Debug.LogFormat("{1} Current Dialogue: {0}", currentConversation.dialogue[0].dialogueLines[0], gameObject.name);
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
