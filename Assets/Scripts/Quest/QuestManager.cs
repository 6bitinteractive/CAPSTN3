using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [HideInInspector]
    public List<Quest> quests = new List<Quest>();

    public Quest CurrentQuest { get; private set; }

    public void Initialize()
    {
        // Get all the quests
        quests.AddRange(GetComponentsInChildren<Quest>());

        foreach (var quest in quests)
            quest.Initialize();
    }

    public void ActivateQuest(int index)
    {
        if (index < 0)
            return;

        if (index < quests.Count)
            quests[index].ActivateQuest();

        CurrentQuest = quests[index];
    }
}
