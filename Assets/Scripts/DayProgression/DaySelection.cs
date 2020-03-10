using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaySelection : MonoBehaviour
{
    private GameManager gameManager;
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        gameManager = SingletonManager.GetInstance<GameManager>();
        buttons.AddRange(GetComponentsInChildren<Button>());

        buttons[gameManager.DayProgression.CurrentDayIndex].interactable = true;
    }

    public void BeginDay()
    {
        gameManager.DayProgression.BeginDay(gameManager.DayProgression.CurrentDayIndex);
    }
}
