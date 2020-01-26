using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Let the DayProgressionManager handle ?
public class QuestManager : Singleton<QuestManager>
{
    [HideInInspector]
    public List<Quest> quests = new List<Quest>();

    // Note: Dummy Data for testing
    //private Quest quest = new Quest(); // Preferably, this (defining the quest details) should be done in the inspector

    private void Start()
    {
        // Get all the quests
        quests.AddRange(GetComponentsInChildren<Quest>());

        // Create the quest events for each quest
        foreach (var quest in quests)
        {
            quest.Initialize();

        }

        // Start first quest
        quests[0].ActivateQuest();
    }
}
