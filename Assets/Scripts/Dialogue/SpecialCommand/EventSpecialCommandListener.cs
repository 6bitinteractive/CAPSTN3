using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSpecialCommandListener : MonoBehaviour
{
    [SerializeField] private string eventName;

    public UnityEvent EventExecuted;

    private static EventCommands eventCommands;
    private EventCommand eventCommand;

    private void Start()
    {
        eventCommands = eventCommands ?? SingletonManager.GetInstance<EventCommands>();
        eventCommand = eventCommands.GetEventCommand(eventName);

        if (eventCommand != null)
            eventCommand.commandEvent.AddListener(OnEventExecuted);
    }

    private void OnEventExecuted()
    {
        if (eventCommand != null)
            EventExecuted.Invoke();
    }
}
