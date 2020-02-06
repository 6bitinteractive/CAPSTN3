using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroupController))]

public class DialogueDisplay : MonoBehaviour
{
    public DialogueSpeaker dialogueSpeaker;
    public TextMeshProUGUI displayText;

    private CanvasGroupController canvasGroup;
    private static DialogueDisplayManager dialogueDisplayManager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupController>();
        displayText.text = string.Empty;
    }

    private void OnEnable()
    {
        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();
        dialogueDisplayManager.dialogueDisplays.Add(dialogueSpeaker, this);
    }

    private void OnDisable()
    {
        dialogueDisplayManager.dialogueDisplays.Remove(dialogueSpeaker);
    }

    private void Start()
    {
        Display(false);
    }

    public void Display(bool value = true)
    {
        canvasGroup.Display(value);
    }

    private void Hide()
    {
        Display(false);
    }
}
