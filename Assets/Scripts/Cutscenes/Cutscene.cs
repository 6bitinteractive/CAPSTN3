using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(Conversation))]

public class Cutscene : MonoBehaviour
{
    public CutsceneEventType OnCutscenePlay;
    public CutsceneEventType OnCutscenePause;
    public CutsceneEventType OnCutsceneStop;

    private PlayableDirector playableDirector;
    private DialogueHandler dialogueHandler;
    private Conversation conversation;

    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        playableDirector = GetComponent<PlayableDirector>();
        dialogueHandler = GetComponent<DialogueHandler>();
        conversation = GetComponent<Conversation>();

        playableDirector.playOnAwake = false;
    }

    public void Play()
    {
        playableDirector.Play();
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutscenePlay, this);
    }

    public void Pause()
    {
        playableDirector.Pause();
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutscenePause, this);
    }

    public void Stop()
    {
        playableDirector.Stop();
        eventManager.Trigger<CutsceneEvent, Cutscene>(OnCutsceneStop, this);
    }
}
