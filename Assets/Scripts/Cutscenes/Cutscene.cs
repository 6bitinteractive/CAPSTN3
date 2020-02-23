using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(DialogueHandler))]

public class Cutscene : MonoBehaviour
{
    [Tooltip("NOTE: PlayOnAwake in PlayableDirector will always be set to false.\n" +
        "We set it here so that events are properly triggered.")]
    [SerializeField] private bool playOnAwake;

    public CutsceneEventType OnCutscenePlay;
    public CutsceneEventType OnCutscenePause;
    public CutsceneEventType OnCutsceneStop;
    public State CurrentState { get; private set; }

    private PlayableDirector playableDirector;
    private DialogueHandler dialogueHandler;
    private List<Conversation> conversations = new List<Conversation>();

    private static EventManager eventManager;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        dialogueHandler = GetComponent<DialogueHandler>();
        conversations.AddRange(GetComponentsInChildren<Conversation>());
        CurrentState = State.Stopped;
    }

    private void OnEnable()
    {
        playableDirector.played += OnPlayableDirectorPlayed;
        playableDirector.paused += OnPlayableDirectorPaused;
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    private void OnDisable()
    {
        playableDirector.played -= OnPlayableDirectorPlayed;
        playableDirector.paused -= OnPlayableDirectorPaused;
        playableDirector.stopped -= OnPlayableDirectorStopped;

        if (CurrentState != State.Stopped)
            Stop();
    }

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();

        if (conversations.Count == 0)
            return;

        // Subscribe to events
        foreach (var conversation in conversations)
        {
            conversation.OnConversationBegin.AddListener(OnConversationBegin);
            conversation.OnConversationEnd.AddListener(OnConversationEnd);

            foreach (var dialogue in conversation.dialogue)
            {
                dialogue.OnDialogueBegin.AddListener(OnDialogueBegin);
                dialogue.OnDialogueEnd.AddListener(OnDialogueEnd);
            }
        }

        if (playOnAwake)
            Play();
    }

    private void OnDestroy()
    {
        if (conversations.Count == 0)
            return;

        foreach (var conversation in conversations)
        {
            conversation.OnConversationBegin.RemoveListener(OnConversationBegin);
            conversation.OnConversationEnd.RemoveListener(OnConversationEnd);

            foreach (var dialogue in conversation.dialogue)
            {
                dialogue.OnDialogueBegin.RemoveListener(OnDialogueBegin);
                dialogue.OnDialogueEnd.RemoveListener(OnDialogueEnd);
            }
        }

    }

    public void Play()
    {
        playableDirector.Play();
    }

    public void Pause()
    {
        playableDirector.Pause();
    }

    public void Stop()
    {
        playableDirector.Stop();
    }

    #region PlayableDirectorEventsCallback
    private void OnPlayableDirectorPlayed(PlayableDirector playableDirector)
    {
        CurrentState = State.Playing;
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutscenePlay, this);
    }

    private void OnPlayableDirectorPaused(PlayableDirector playableDirector)
    {
        CurrentState = State.Paused;
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutscenePause, this);
    }

    private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
    {
        CurrentState = State.Stopped;
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutsceneStop, this);
    }
    #endregion

    private void OnDialogueBegin()
    {
    }

    // NOTE: OnDialogueEnd is invoked right away and does not wait for player input
    // Depending on your needs, this might not be the best place to control the PlayableDirector
    private void OnDialogueEnd()
    {
    }

    private void OnConversationBegin()
    {
        Pause();
    }

    private void OnConversationEnd()
    {
        if (CurrentState == State.Stopped)
            return;

        Play();
    }

    public enum State
    {
        Stopped,
        Playing,
        Paused
    }
}
