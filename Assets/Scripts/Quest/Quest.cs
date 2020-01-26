using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour // Only a MonoBehaviour to make it available in the inspector
{
    // [SerializeField] private
    public List<QuestEvent> questEvents = new List<QuestEvent>();

    public void Initialize()
    {
        DefinePath();
        DefineOrder(questEvents[0].Id);
        PrintPath();
    }

    public void ActivateQuest()
    {
        questEvents[0].SwitchStatus(QuestEvent.Status.Active);
    }

    // For debugging
    public void PrintPath()
    {
        Debug.Log("Quest Path:");
        for (int i = 0; i < questEvents.Count; i++)
            Debug.LogFormat("({0} | Depth {3}): {1} - {2}", i, questEvents[i].DisplayName, questEvents[i].CurrentStatus, questEvents[i].order);
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
