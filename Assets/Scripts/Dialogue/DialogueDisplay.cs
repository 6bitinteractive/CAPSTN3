using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupController))]

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private Transform canvasContainer;
    [SerializeField] private GameObject dialogueButtonPanel;

    public DialogueSpeaker dialogueSpeaker;
    public TextMeshProUGUI displayText;

    private Transform thisTransform;
    private bool displayed;

    private Button dialogueButton;
    private CanvasGroupController canvasGroup;

    private static DialogueDisplayManager dialogueDisplayManager;
    private static Camera mainCam;

    private void Awake()
    {
        thisTransform = transform;
        dialogueButton = dialogueButtonPanel.GetComponentInChildren<Button>();
        canvasGroup = GetComponent<CanvasGroupController>();
        displayText.text = string.Empty;
    }

    private void OnEnable()
    {
        dialogueDisplayManager = dialogueDisplayManager ?? SingletonManager.GetInstance<DialogueDisplayManager>();
        dialogueButton.onClick.AddListener(dialogueDisplayManager.ContinueConversation);
        dialogueDisplayManager.dialogueDisplays.Add(dialogueSpeaker, this);
    }

    private void OnDisable()
    {
        dialogueButton.onClick.RemoveListener(dialogueDisplayManager.ContinueConversation);
        dialogueDisplayManager.dialogueDisplays.Remove(dialogueSpeaker);
    }

    private void Start()
    {
        mainCam = SingletonManager.GetInstance<CameraManager>().MainCam;
        Display(false);
    }

    private void LateUpdate()
    {
        if (displayed)
            Reposition();
    }

    public void Display(bool value = true)
    {
        displayed = value;
        canvasGroup.Display(value);
    }

    public void ShowButton(bool value = true)
    {
        dialogueButtonPanel.SetActive(value);
    }

    private void Hide()
    {
        Display(false);
    }

    private void Reposition()
    {
        thisTransform.position = mainCam.WorldToScreenPoint(canvasContainer.position);
    }
}
