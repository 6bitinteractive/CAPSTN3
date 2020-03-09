#if UNITY_EDITOR
using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDebug : MonoBehaviour
{
    [SerializeField] private int startDayIndex;
    [SerializeField] private DayProgression dayProgression;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = SingletonManager.GetInstance<GameManager>();

        if (!gameManager.debug)
            return;

        //dayProgression.Initialize();

        //// Force complete all quest event prior to dayIndex
        //for (int i = 0; i < startDayIndex; i++)
        //    dayProgression.QuestManager.questCollections[i].ForceComplete();

        // TODO: Force complete inactive quest events


        // Start
        dayProgression.BeginDay(startDayIndex);
    }
}
#endif
