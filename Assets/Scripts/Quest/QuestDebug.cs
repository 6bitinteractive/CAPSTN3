using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDebug : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private int startDayIndex;
    [SerializeField] private DayProgression dayProgression;

    private void Start()
    {
        if (!debugMode)
            return;

        dayProgression.debugMode = true;
        dayProgression.Initialize();

        // Force complete all quest event prior to dayIndex
        for (int i = 0; i < startDayIndex; i++)
            dayProgression.QuestManager.quests[i].ForceComplete();

        // TODO: Force complete inactive quest events


        // Start
        dayProgression.BeginDay(startDayIndex);
    }
}
