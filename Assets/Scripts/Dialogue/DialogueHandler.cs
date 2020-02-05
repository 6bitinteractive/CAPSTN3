using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// NOTE: Be careful when setting up QuestEvents that are in succession
// Ex. When QuestEvent A is done, it would mean that QuestEventB will start next right away (given current implentation)...
// This means that the dialogue set for QuestEventA for a done status will not be used unless QuestEventB is started
// at a much later time (e.g. manually activating it instead of switching it to active soon after the previous QuestEvent has been done)

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private Conversation defaultConversation;
    [SerializeField] private List<ConversationSet> questRelatedConversations;

    public Conversation CurrentConversation { get; private set; }

    private static EventManager eventManager;
    private static DialogueDisplayManager dialogueDisplayManager;
    private bool isDirty;

    public static UnityEvent OnConversationEnd = new UnityEvent();

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

    private bool startingNewConversation = true;
    private bool closeConversation;
    private Dialogue dialogue;

    public void DisplayCurrentConversation()
    {
        // When we're done going through the conversation but not yet ready to start a new one (i.e. we haven't been able to hide the dialogue display)
        if (closeConversation && !startingNewConversation)
        {
            OnConversationEnd.Invoke(); // The display should be listening to this event and hide itself once the event has been invoked
            startingNewConversation = true; // Now, we can let the player start a new conversation
            return;
        }

        if (startingNewConversation)
        {
            startingNewConversation = false; // We make sure we keep continuing...
            closeConversation = false; // ...and not yet end the conversation

            CurrentConversation.ResetConversation(); // We make sure that if the conversation is still the same, we loop back to the first dialogue
            dialogue = CurrentConversation.GetNextDialogue();
        }

        string nextLine = dialogue.GetNextLine();
        var display = dialogueDisplayManager.GetDialogueDisplay(dialogue.speaker);
        display.Display();
        display.displayText.text = nextLine;

        if (dialogue.NextIsEnd()) // Check if we're at the end of the current speaker's dialogue
        {
            if (!CurrentConversation.NextIsEnd()) // Check if we're not yet at the end of the conversation
            {
                dialogue = CurrentConversation.GetNextDialogue();
            }
            else
            {
                closeConversation = true;
                return; // Conversation has ended
            }
        }
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
