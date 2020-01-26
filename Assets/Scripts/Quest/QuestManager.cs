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

        //// TEST
        //// Create events
        //QuestEvent a = quest.AddQuestEvent("event 1", "event des 1");
        //QuestEvent b = quest.AddQuestEvent("event 2", "event des 2");
        //QuestEvent c = quest.AddQuestEvent("event 3", "event des 3");
        //QuestEvent d = quest.AddQuestEvent("event 4", "event des 4");

        //// TEST
        //// Define the paths, i.e. the order of the events
        //quest.DefinePath();
        //quest.DefineOrder(a.Id);

        //// TEST
        //// Listen when a quest event ends
        //foreach (var questEvent in quest.questEvents)
        //{
        //    questEvent.OnDone.AddListener(UpdateOnQuestEventDone);
        //}

        //// TEST
        //// Begin quest
        //quest.ActivateQuest();

        //// TEST
        //// Check path/order
        //quest.PrintPath();

        //// TEST
        //quest.questEvents[0].SwitchStatus(QuestEvent.Status.Done);

        //quest.PrintPath();
    }

    private void UpdateOnQuestEventDone(QuestEvent doneQuestEvent)
    {
        //foreach (var questEvent in quest.questEvents)
        //{
        //    // If this event is next in order
        //    if (questEvent.order == doneQuestEvent.order + 1)
        //    {
        //        // Start the next quest event in line
        //        questEvent.SwitchStatus(QuestEvent.Status.Active);
        //    }
        //}
    }
}
