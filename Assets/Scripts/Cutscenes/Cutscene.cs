using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(DialogueHandler))]

public class Cutscene : MonoBehaviour
{
    public CutsceneEventType OnCutscenePlay;
    public CutsceneEventType OnCutscenePause;
    public CutsceneEventType OnCutsceneStop;

    private PlayableDirector playableDirector;
    private DialogueHandler dialogueHandler;
    private List<Conversation> conversations = new List<Conversation>();

    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        playableDirector = GetComponent<PlayableDirector>();
        dialogueHandler = GetComponent<DialogueHandler>();
        conversations.AddRange(GetComponentsInChildren<Conversation>());

        playableDirector.playOnAwake = false;

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

    private void OnDialogueBegin()
    {
    }

    private void OnDialogueEnd()
    {
    }

    private void OnConversationBegin()
    {
        Pause();
    }

    private void OnConversationEnd()
    {
        Play();
    }
}
