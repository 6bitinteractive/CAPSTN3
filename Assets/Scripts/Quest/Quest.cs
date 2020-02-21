using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

public class Quest : MonoBehaviour // Only a MonoBehaviour to make it available in the inspector
{
    private List<QuestEvent> questEvents = new List<QuestEvent>();

    // Assumes there's only one active quest event which is true for current implementation
    public QuestEvent CurrentQuestEvent { get; private set; }  // The current active
    public QuestEvent PreviousQuestEvent { get; private set; } // The recently done

    public UnityEvent OnQuestEnd = new UnityEvent();

    public void Initialize()
    {
        questEvents.AddRange(GetComponentsInChildren<QuestEvent>());

        foreach (var questEvent in questEvents)
        {
            questEvent.Initialize();
            questEvent.OnDone.gameEvent.AddListener(EvaluateQuestState);
        }

        DefinePath();
        DefineOrder(questEvents[0].Id);
        PrintPath();
    }

    // NOTE: This is mainly used for debugging!
    public void ForceComplete()
    {
        foreach (var questEvent in questEvents)
            questEvent.ForceComplete();

        gameObject.SetActive(false);
    }

    private void EvaluateQuestState(QuestEvent doneQuestEvent)
    {
        foreach (var questEvent in questEvents)
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
        if (doneQuestEvent == questEvents[questEvents.Count - 1])
        {
            Debug.Log("All quest events have been completed.");
            EndQuest();
        }
    }

    public void ActivateQuest()
    {
        questEvents[0].SwitchStatus(QuestEvent.Status.Active);
    }

    public void EndQuest()
    {
        foreach (var questEvent in questEvents)
            questEvent.OnDone.gameEvent.RemoveListener(EvaluateQuestState);

        OnQuestEnd.Invoke();
    }

    // For debugging
    public void PrintPath()
    {
        Debug.Log("Quest Path:");
        for (int i = 0; i < questEvents.Count; i++)
            Debug.LogFormat("|- ({0} | Depth {3}): {1} - {2}", i, questEvents[i].DisplayName, questEvents[i].CurrentStatus, questEvents[i].order);
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
        foreach (var questEvent in questEvents)
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
        for (int i = 0; i < questEvents.Count; i++)
        {
            if (i + 1 >= questEvents.Count)
                return;

            AddPath(questEvents[i].Id, questEvents[i + 1].Id);
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
