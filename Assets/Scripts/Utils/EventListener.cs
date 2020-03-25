using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [Header("Listen for")]
    [SerializeField] private bool enterArea;
    [SerializeField] private bool cutscene;

    public UnityEvent OnEventStart;
    public UnityEvent OnEventEnd;

    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        if (enterArea) eventManager.Subscribe<EnterAreaEvent, EnterArea>(OnEnterArea);
        if (cutscene) eventManager.Subscribe<CutsceneEvent, Cutscene>(OnCutscene);
    }

    private void OnDestroy()
    {
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
