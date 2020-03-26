using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [HideInInspector]
    public List<QuestCollection> questCollections;

    public QuestCollection CurrentQuestCollection { get; private set; }

    public void Initialize()
    {
        // Get all the quests
        questCollections = new List<QuestCollection>();
        questCollections.AddRange(GetComponentsInChildren<QuestCollection>());

        foreach (var questCollection in questCollections)
            questCollection.Initialize();
    }

    public void ActivateQuestCollection(int index)
    {
        if (index < 0)
            return;

        if (index < questCollections.Count)
            questCollections[index].ActivateQuest();

        CurrentQuestCollection = questCollections[index];
    }
}
