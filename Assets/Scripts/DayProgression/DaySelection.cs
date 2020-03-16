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

        // Less 1 because the storybook scene starts with day 2 (which is index 1)
        buttons[gameManager.DayProgression.CurrentDayIndex - 1].interactable = true;
    }
}
