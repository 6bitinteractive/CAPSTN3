using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Display(bool value = true)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.blocksRaycasts = value;
        canvasGroup.interactable = value;
    }
}
