using System.Collections;
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

    private static EventManager eventManager;
    private static DialogueDisplayManager dialogueDisplayManager;
    private bool isDirty;



    private IEnumerator Start()
    {
        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();

        // Listen to quest event status update
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
        //Debug.LogFormat("DialogueHandler for {0} started listening to QuestEventUpdates.", gameObject.name);

        yield return new WaitForEndOfFrame();

        // TODO: Load a saved data

        CurrentConversation = defaultConversation;
        QuestEvent currentQuestEvent = SingletonManager.GetInstance<QuestManager>().CurrentQuest.CurrentQuestEvent;
        if (!isDirty) // We only set a conversation at Start() if no QuestEvent has changed it yet.
        {
            //Debug.LogFormat("Set default conversation for {0}", gameObject.name);
            DetermineCurrentConversation(currentQuestEvent);
        }
    }

    private void OnDisable()
    {
        //Debug.LogFormat("DialogueHandler for {0} stopped listening to QuestEventUpdates.", gameObject.name);
        eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(DetermineCurrentConversation);
    }

    public void StartConversation()
    {
        // Feed current conversation to DialogueDisplayManager
        dialogueDisplayManager.DisplayConversation(CurrentConversation);
    }

    public void SwitchConversation(Conversation conversation)
    {
        CurrentConversation = conversation;
    }








    private void DetermineCurrentConversation(QuestEvent questEvent)
    {
        isDirty = true;

        // Find the related conversation to the quest event
        // If there's no related conversation to the questEvent, we set current conversation to default
        // TODO: Cache QuestEvent component instead of always calling it every evaluation

        //Debug.Log("QUESTEVENT: " + questEvent.DisplayName + " - " + questEvent.CurrentStatus);
        ConversationSet cs = questRelatedConversations.Find(x => questEvent == x.questEventReference.gameObject.GetComponent<QuestEvent>()
                                                          && questEvent.CurrentStatus == x.requiredQuestEventStatus);

        if (cs != null)
        {
            CurrentConversation = cs.conversation;
            Debug.LogFormat("Dialogue updated by {0}", questEvent);
        }
        else
        {
            CurrentConversation = defaultConversation;
        }

        Debug.LogFormat("{1} Current Dialogue: {0}", CurrentConversation.dialogue[0].dialogueLines[0], gameObject.name);
    }

    [System.Serializable]
    public class ConversationSet
    {
        public GuidReference questEventReference;
        public QuestEvent.Status requiredQuestEventStatus = QuestEvent.Status.Active;
        public Conversation conversation;
    }
}
