using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private Conversation defaultConversation;
    [SerializeField] private List<ConversationSet> questRelatedConversations;

    public Conversation CurrentConversation { get; private set; }

    private static EventManager eventManager;
    private bool isDirty;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Debug.LogFormat("DialogueHandler for {0} started listening to QuestEventUpdates.", gameObject.name);
        // Listen to quest event status update
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);

        // TODO: Load a saved data

        // TEST
        QuestEvent currentQuestEvent = SingletonManager.GetInstance<QuestManager>().CurrentQuest.CurrentQuestEvent;
        if (!isDirty) // We only set a conversation at Start() if no QuestEvent has changed it yet.
        {
            Debug.Log("Set default conversation");
            DetermineCurrentConversation(currentQuestEvent);
        }
        // ----
    }

    private void OnDisable()
    {
        Debug.LogFormat("DialogueHandler for {0} stopped listening to QuestEventUpdates.", gameObject.name);
        eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
    }

    private void DetermineCurrentConversation(QuestEvent questEvent)
    {
        isDirty = true;

        // Find the related conversation to the quest event
        // If there's no related conversation to the questEvent, we set current conversation to default
        // TODO: Cache QuestEvent component instead of always calling it every evaluation

        CurrentConversation = defaultConversation;
        ConversationSet cs = questRelatedConversations.Find(x => questEvent == x.questEventReference.gameObject.GetComponent<QuestEvent>()
                                                          && questEvent.CurrentStatus == x.requiredQuestEventStatus);

        if (cs != null)
        {
            CurrentConversation = cs.conversation;
            Debug.LogFormat("Dialogue updated by {0}", questEvent);
        }
        else
        {
        }

        Debug.LogFormat("Current Dialogue: {0}", CurrentConversation.dialogue[0].dialogueLines[0]);
    }

    [System.Serializable]
    public class ConversationSet
    {
        public GuidReference questEventReference;
        public QuestEvent.Status requiredQuestEventStatus;
        public Conversation conversation;
    }
}
