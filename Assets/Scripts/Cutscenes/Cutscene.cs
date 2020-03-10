using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// NOTE: Make sure to use Stop() in the inspector for Cutscenes that playOnAwake
// Simply disabling them might not cleanly stop the cutscene

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(DialogueHandler))]

public class Cutscene : Persistable<CutsceneData>
{
    [Tooltip("NOTE: PlayOnAwake in PlayableDirector will always be set to false.\n" +
        "We set it here so that events are properly triggered.\n\n" +
        "It's advisable to avoid playing cutscenes on awake but, instead, " +
        "manually call the Play() method in a QuestState/ConditionState listener")]
    [SerializeField] private bool playOnAwake;

    [Tooltip("Cutscene can be played again even after being viewed once.")]
    [SerializeField] private bool replayable;

    public CutsceneEventType OnCutscenePlay;
    public CutsceneEventType OnCutscenePause;
    public CutsceneEventType OnCutsceneStop;
    public State CurrentState { get; private set; }
    public int PlayCount { get; private set; } // Number of times the cutscene has been completely viewed

    private PlayableDirector playableDirector;
    private DialogueHandler dialogueHandler;
    private List<Conversation> conversations = new List<Conversation>();

    private static EventManager eventManager;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        dialogueHandler = GetComponent<DialogueHandler>();
        conversations.AddRange(GetComponentsInChildren<Conversation>());

        playableDirector.playOnAwake = false;
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
        InitializeData();

        // If the cutscene is not replayable and has been played (count is 1+)
        if (!replayable && PlayCount >= 1)
            return;

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
        PlayCount++;
        CurrentState = State.Stopped;
        UpdatePersistentData(); // This needs to be updated first as triggering the event will call to save the data
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

    public override CutsceneData GetPersistentData()
    {
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        PlayCount = Data.playCount;
        //Debug.LogFormat("CUTSCENE \"{0}\" played x{1}", gameObject.name, Data.playCount);
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.playCount = PlayCount;
        gameManager.GameData.AddPersistentData(Data);
        //Debug.LogFormat("CUTSCENE \"{0}\" played x{1}", gameObject.name, Data.playCount);
    }
}
