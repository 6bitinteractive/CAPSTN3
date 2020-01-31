using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    public DialogueSpeaker dialogueSpeaker;
    public TextMeshProUGUI displayText;

    private CanvasGroupController canvasGroup;
    private static DialogueDisplayManager dialogueDisplayManager;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroupController>();
        Display(false);

        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();
        dialogueDisplayManager.dialogueDisplays.Add(this);

        DialogueHandler.OnConversationEnd.AddListener(Hide);
    }

    private void OnDisable()
    {
        DialogueHandler.OnConversationEnd.RemoveListener(Hide);
        dialogueDisplayManager.dialogueDisplays.Remove(this);
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
