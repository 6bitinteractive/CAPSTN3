using Meowfia.WanderDog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [Header("Reaction Start")]
    [Header("Listen for")]
    [SerializeField] private bool sceneTransition;
    [SerializeField] private bool enterArea;
    [SerializeField] private bool cutscene;
    [SerializeField] private bool dayEnd;

    [Header("Reaction End")]
    [SerializeField] private bool dayBegin;

    public UnityEvent OnEventStart;
    public UnityEvent OnEventEnd;

    private static GameManager gameManager;
    private static SceneController sceneController;
    private static EventManager eventManager;

    private void Start()
    {
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
        if (dayBegin) gameManager.DayProgression.OnDayBegin.AddListener(ReactEnd);
        if (dayEnd) gameManager.DayProgression.OnDayEnd.AddListener(ReactStart);

        sceneController = sceneController ?? SingletonManager.GetInstance<SceneController>();
        if (sceneTransition) sceneController.BeforePreviousSceneUnload.AddListener(ReactStart);

        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        if (enterArea) eventManager.Subscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        if (cutscene) eventManager.Subscribe<CutsceneEvent, Cutscene>(OnCutscene);
    }

    private void OnDestroy()
    {
        if (dayBegin) gameManager.DayProgression.OnDayBegin.RemoveListener(ReactEnd);
        if (dayEnd) gameManager.DayProgression.OnDayEnd.RemoveListener(ReactStart);
        if (sceneTransition) sceneController.BeforePreviousSceneUnload.RemoveListener(ReactStart);
        if (enterArea) eventManager.Unsubscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        if (cutscene) eventManager.Unsubscribe<CutsceneEvent, Cutscene>(OnCutscene);
    }

    private void OnCutscene(Cutscene cutscene)
    {
        if (cutscene.CurrentState == Cutscene.State.Playing)
            ReactStart();
        else if (cutscene.CurrentState == Cutscene.State.Stopped)
            ReactEnd();
    }

    private void OnEnterArea(EnterArea enterArea)
    {
        ReactStart();
    }

    private void ReactStart()
    {
        OnEventStart.Invoke();
    }

    private void ReactEnd()
    {
        OnEventEnd.Invoke();
    }
}
