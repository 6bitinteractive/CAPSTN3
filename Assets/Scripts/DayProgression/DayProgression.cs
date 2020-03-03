using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgression : MonoBehaviour
{
    // "Proper" count of day, starting with 1
    public int CurrentDayCount => CurrentDayIndex + 1;
    public int CurrentDayIndex { get; set; }
    public QuestManager QuestManager { get; private set; }

    [HideInInspector]
    public bool debugMode;

    private void Start()
    {
        if (debugMode)
            return;

        Initialize();
        BeginDay(CurrentDayIndex);
    }

    public void Initialize()
    {
        QuestManager = SingletonManager.GetInstance<QuestManager>();
        QuestManager.Initialize();

        foreach (var quest in QuestManager.quests)
        {
            quest.OnQuestEnd.AddListener(EndDay); // Test only; EndDay() will most probably be manually called
        }
    }

    public void BeginDay(int index = 0)
    {
        CurrentDayIndex = index;
        Debug.LogFormat("Beginning Day {0}", CurrentDayCount);

        // Start the quest to be tackled for the day; assumes that the day shares the same index number as the quest
        QuestManager.ActivateQuest(CurrentDayIndex);
    }

    public void EndDay()
    {
        Debug.LogFormat("Finished Day {0}", CurrentDayCount);
        CurrentDayIndex++;
    }
}
