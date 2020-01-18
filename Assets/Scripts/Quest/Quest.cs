using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private List<QuestEvent> questEvents = new List<QuestEvent>();

    private List<QuestEvent> pathList = new List<QuestEvent>();

    // TODO: same as QuestEvent?

    public Quest() { }

    public QuestEvent AddQuestEvent(string name, string description)
    {
        QuestEvent questEvent = new QuestEvent(name, description);
        questEvents.Add(questEvent);

        return questEvent;
    }

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
            from.pathList.Add(path);        }
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

    public void DefinePath()
    {
        // This is a simple linear progression
        for (int i = 0; i < questEvents.Count; i++)
        {
            if (i + 1 >= questEvents.Count)
                return;

            AddPath(questEvents[i].Id, questEvents[i + 1].Id);
        }
    }

    public void ActivateQuest()
    {
        questEvents[0].CurrentStatus = QuestEvent.Status.Active;
    }

    // For debugging
    public void PrintPath()
    {
        Debug.Log("Quest Path:");
        for (int i = 0; i < questEvents.Count; i++)
            Debug.LogFormat("({0}): {1} - {2}", i, questEvents[i].DisplayName, questEvents[i].CurrentStatus);
    }
}
