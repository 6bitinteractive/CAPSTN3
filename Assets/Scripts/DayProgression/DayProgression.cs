using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgression : MonoBehaviour
{
    // "Proper" count of day, starting with 1
    public int CurrentDayCount => currentDay + 1;

    private QuestManager questManager;

    //[SerializeField]
    private int currentDay;

    private void Start()
    {
        questManager = SingletonManager.GetInstance<QuestManager>();
        questManager.Initialize();

        foreach (var quest in questManager.quests)
        {
            quest.OnQuestEnd.AddListener(EndDay); // Test
        }

        BeginDay();
    }

    public void BeginDay()
    {
        Debug.LogFormat("Beginning Day {0}", CurrentDayCount);
        // Start the quest to be tackled for the day
        questManager.ActivateQuest(currentDay);
    }

    public void EndDay()
    {
        Debug.LogFormat("Finished Day {0}", CurrentDayCount);
        currentDay++;
    }
}
