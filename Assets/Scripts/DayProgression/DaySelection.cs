using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DaySelection : MonoBehaviour
{
    // FOR TESTING ONLY
    [SerializeField] private EventSystem eventSystem;

    private GameManager gameManager;
    private List<Button> buttons = new List<Button>();

    private void Start()
    {
        gameManager = SingletonManager.GetInstance<GameManager>();
        buttons.AddRange(GetComponentsInChildren<Button>());

        // Less 1 because the storybook scene starts with day 2 (which is index 1)
        Button buttonForCurrentDay = buttons[gameManager.DayProgression.CurrentDayIndex - 1];
        buttonForCurrentDay.interactable = true;
        eventSystem.SetSelectedGameObject(buttonForCurrentDay.gameObject, null);
    }
}
