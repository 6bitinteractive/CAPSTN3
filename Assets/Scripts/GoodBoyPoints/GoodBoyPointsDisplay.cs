using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodBoyPointsDisplay : MonoBehaviour
{
    [SerializeField] private GoodBoyPointsHandler goodBoyPoints;
    [SerializeField] private Image valueImage;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color badColor;

    private void OnEnable()
    {
        goodBoyPoints.OnEvaluateGoodBoyPoints.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        goodBoyPoints.OnEvaluateGoodBoyPoints.RemoveListener(UpdateDisplay);
    }

    private void UpdateDisplay(GoodBoyPointsHandler goodBoyPoints)
    {
        valueImage.fillAmount = goodBoyPoints.CurrentValueNormalized;

        switch (goodBoyPoints.CurrentState)
        {
            case GoodBoyPointsHandler.State.Good:
                valueImage.color = goodColor;
                break;
            case GoodBoyPointsHandler.State.Normal:
                valueImage.color = normalColor;
                break;
            case GoodBoyPointsHandler.State.Bad:
                valueImage.color = badColor;
                break;
        }
    }
}
