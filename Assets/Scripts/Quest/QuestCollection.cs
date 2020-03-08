using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

// QuestCollection holds all the quests (QuestEvents) related to a specific day
public class QuestCollection : Persistable // Only a MonoBehaviour to make it available in the inspector
{
    public List<QuestEvent> QuestEvents { get; } = new List<QuestEvent>();

    // Assumes there's only one active quest event which is true for current implementation
    public QuestEvent CurrentQuestEvent { get; private set; }  // The current active
    public QuestEvent PreviousQuestEvent { get; private set; } // The recently done

    public UnityEvent OnQuestEnd = new UnityEvent();

    public void Initialize()
    {
        QuestEvents.AddRange(GetComponentsInChildren<QuestEvent>());

        foreach (var questEvent in QuestEvents)
        {
            questEvent.Initialize();
            questEvent.OnDone.gameEvent.AddListener(EvaluateQuestState);
        }

        DefinePath();
        DefineOrder(QuestEvents[0].Id);
        //PrintPath();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        foreach (var questEvent in QuestEvents)
            questEvent.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        foreach (var questEvent in QuestEvents)
            questEvent.Load(reader);
    }

    // NOTE: This is mainly used for debugging!
    public void ForceComplete()
    {
        foreach (var questEvent in QuestEvents)
            questEvent.ForceComplete();

        gameObject.SetActive(false);
    }

    private void EvaluateQuestState(QuestEvent doneQuestEvent)
    {
        foreach (var questEvent in QuestEvents)
        {
            // If this event is next in order
            if (questEvent.order == doneQuestEvent.order + 1)
            {
                // Start the next quest event in line
                questEvent.SwitchStatus(QuestEvent.Status.Active);

                // Set as current quest event
                CurrentQuestEvent = questEvent;
            }
        }

        // Set done as previous quest event
        PreviousQuestEvent = doneQuestEvent;

        // Turn off recently done quest event
        doneQuestEvent.gameObject.SetActive(false);

        // Note: For now, we assume that all quest events will be done
        // ... so once we finish the last quest event, we can then end the Quest.
        if (doneQuestEvent == QuestEvents[QuestEvents.Count - 1])
        {
            Debug.Log("All quest events have been completed.");
            EndQuest();
        }
    }

    public void ActivateQuest()
    {
        QuestEvent firstInactiveQuest = QuestEvents.Find(x => x.CurrentStatus == QuestEvent.Status.Inactive);
        firstInactiveQuest.SwitchStatus(QuestEvent.Status.Active);
        CurrentQuestEvent = firstInactiveQuest;
    }

    public void EndQuest()
    {
        foreach (var questEvent in QuestEvents)
            questEvent.OnDone.gameEvent.RemoveListener(EvaluateQuestState);

        OnQuestEnd.Invoke();
    }

    // For debugging
    public void PrintPath()
    {
        Debug.Log("Quest Path:");
        for (int i = 0; i < QuestEvents.Count; i++)
            Debug.LogFormat("|- ({0} | Depth {3}): {1} - {2}", i, QuestEvents[i].DisplayName, QuestEvents[i].CurrentStatus, QuestEvents[i].order);
    }

    //public QuestEvent AddQuestEvent(string name, string description)
    //{
    //    QuestEvent questEvent = new QuestEvent(name, description);
    //    questEvents.Add(questEvent);

    //    return questEvent;
    //}

    /// <summary>
    /// Add to list of paths.
    /// </summary>
    /// <param name="fromQuestEvent">Unique Id of the "from" quest event.</param>
    /// <param name="toQuestEvent">Unique Id of the "to" quest event.</param>
    public void AddPath(string fromQuestEvent, string toQuestEvent)
    {
        QuestEvent from = FindQuestEvent(fromQuestEvent);
        QuestEvent to = FindQuestEvent(toQuestEvent);

        if (from != null && to != null)
        {
            QuestPath path = new QuestPath(from, to);
            from.pathList.Add(path);
        }
    }

    private QuestEvent FindQuestEvent(string id)
    {
        foreach (var questEvent in QuestEvents)
        {
            if (questEvent.Id == id)
                return questEvent;
        }

        return null;
    }

    /// <summary>
    /// Define the path/connection between two quest events.
    /// </summary>
    private void DefinePath()
    {
        // This is a simple linear progression, i.e. quests are done sequentially
        for (int i = 0; i < QuestEvents.Count; i++)
        {
            if (i + 1 >= QuestEvents.Count)
                return;

            AddPath(QuestEvents[i].Id, QuestEvents[i + 1].Id);
        }
    }

    /// <summary>
    /// Define the order of the quest events.
    /// </summary>
    /// <param name="id">The Id of the quest event. Typically, the root (i.e. very first startEvent) quest event is first passed to the function.</param>
    /// <param name="orderNumber">The order of the quest event.</param>
    private void DefineOrder(string id, int orderNumber = 1)
    {
        // Use Breadth-First Search to traverse the paths and indicate the order
        // BFS allows us to traverse all the nodes

        QuestEvent thisEvent = FindQuestEvent(id);
        thisEvent.order = orderNumber; // Starting node gets the default orderNumber, 1

        foreach (var pathNode in thisEvent.pathList)
        {
            if (pathNode.endEvent.order == -1)
                DefineOrder(pathNode.endEvent.Id, orderNumber + 1); // Recursively call the function;
                                                                    // The endEvent of the current quest then becomes the startEvent and subsequently increases the order number.
        }
    }
}
