using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Let the DayProgressionManager handle ?
public class QuestManager : MonoBehaviour
{
    // Note: Dummy Data for testing
    private Quest quest = new Quest(); // Preferably, this (defining the quest details) should be done in the inspector

    private void Start()
    {
        // TEST
        // Create events
        QuestEvent a = quest.AddQuestEvent("event 1", "event des 1");
        QuestEvent b = quest.AddQuestEvent("event 2", "event des 2");
        QuestEvent c = quest.AddQuestEvent("event 3", "event des 3");
        QuestEvent d = quest.AddQuestEvent("event 4", "event des 4");

        // TEST
        // Define the paths, i.e. the order of the events
        quest.DefinePath();

        // TEST
        // Begin quest
        quest.ActivateQuest();

        // Debug
        quest.PrintPath();
    }
}
