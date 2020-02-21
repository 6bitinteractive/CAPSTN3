using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgression : MonoBehaviour
{
    // "Proper" count of day, starting with 1
    public int CurrentDayCount => currentDayIndex + 1;
    public QuestManager QuestManager { get; private set; }

    [HideInInspector]
    public bool debugMode;

    private int currentDayIndex;

    private void Start()
    {
        if (debugMode)
            return;

        Initialize();
        BeginDay(0);
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
        currentDayIndex = index;
        Debug.LogFormat("Beginning Day {0}", CurrentDayCount);

        // Start the quest to be tackled for the day; assumes that the day shares the same index number as the quest
        QuestManager.ActivateQuest(currentDayIndex);
    }

    public void EndDay()
    {
        Debug.LogFormat("Finished Day {0}", CurrentDayCount);
        currentDayIndex++;
    }
}
