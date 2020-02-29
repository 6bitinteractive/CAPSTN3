using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerDisplay : MonoBehaviour
{
    [SerializeField] private Hunger hunger;
    [SerializeField] private Image valueImage;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color hungryColor;

    private void OnEnable()
    {
        hunger.OnEvaluateHunger.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        hunger.OnEvaluateHunger.RemoveListener(UpdateDisplay);
    }

    private void UpdateDisplay(Hunger hunger)
    {
        valueImage.fillAmount = hunger.CurrentValueNormalized;

        switch (hunger.CurrentState)
        {
            case Hunger.State.Full:
                valueImage.color = fullColor;
                break;
            case Hunger.State.Normal:
                valueImage.color = normalColor;
                break;
            case Hunger.State.Hungry:
                valueImage.color = hungryColor;
                break;
        }
    }
}
