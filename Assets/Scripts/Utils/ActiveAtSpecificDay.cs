using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: This is not a good way to enable/disable quest-related objects
// since those can be enabled/disabled as quests are updated within the day

public class ActiveAtSpecificDay : MonoBehaviour
{
    [Tooltip("Not the index.")]
    [SerializeField] private int day;

    private static DayProgression dayProgression;

    private void Start()
    {
        if (day <= 0)
            Debug.LogError("Day cannot be 0 or negative");

        dayProgression = dayProgression ?? SingletonManager.GetInstance<GameManager>().DayProgression;
        //Debug.Log(dayProgression.CurrentDayCount);
        gameObject.SetActive(day == dayProgression.CurrentDayCount);
    }
}
