using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgression : Persistable<PersistentData>
{
    /// <summary>
    /// This is currentDayIndex + 1, i.e. the human-readable/natural way of counting days
    /// </summary>
    public int CurrentDayCount => CurrentDayIndex + 1;

    /// <summary>
    /// Day 1 is index 0.
    /// </summary>
    public int CurrentDayIndex { get; set; }

    private QuestManager questManager;

    public void Initialize(QuestManager qm)
    {
        questManager = qm;
        questManager.Initialize();

        foreach (var quest in questManager.questCollections)
        {
            quest.OnQuestEnd.AddListener(EndDay); // Test only; EndDay() will most probably be manually called
        }
    }

    public void BeginDay(int index = 0)
    {
        CurrentDayIndex = index;
        Debug.LogFormat("Beginning Day {0}", CurrentDayCount);

        // Start the quest to be tackled for the day; assumes that the day shares the same index number as the quest
        questManager.ActivateQuestCollection(CurrentDayIndex);
    }

    public void EndDay()
    {
        Debug.LogFormat("Finished Day {0}", CurrentDayCount);
        CurrentDayIndex++;
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(CurrentDayIndex);
    }

    public override void Load(GameDataReader reader)
    {
        CurrentDayIndex = reader.ReadInt();
    }
}
