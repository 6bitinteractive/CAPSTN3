using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager : MonoBehaviour
{
    [Tooltip("Not the index.")]
    [SerializeField] private int day;

    private DayProgression dayProgression;

    private void Start()
    {
        if (day <= 0)
            Debug.LogError("Day cannot be 0 or negative");

        dayProgression = SingletonManager.GetInstance<GameManager>().DayProgression;
        Debug.Log(dayProgression.CurrentDayCount);
        gameObject.SetActive(day == dayProgression.CurrentDayCount);
    }
}
