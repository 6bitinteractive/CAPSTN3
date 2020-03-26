using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// {event:eventName}
// Fix: Commands at the end of the line don't work properly
// It's best to place the command in front of a character---e.g. "Can you jump{event:bark}?" as opposed to "Can you jump?{event:bark}"
// As a hack, place a space after the {event:x} so that it will still execute the command

public class EventCommands : Singleton<EventCommands>
{
    public List<EventCommand> eventCommands;

    private Dictionary<string, EventCommand> eventCommandsDict;

    private void Initialize()
    {
        eventCommandsDict = new Dictionary<string, EventCommand>();
        foreach (var item in eventCommands)
            eventCommandsDict.Add(item.eventName, item);
    }

    public EventCommand GetEventCommand(string eventName)
    {
        if (eventCommandsDict == null) Initialize();

        if (eventCommandsDict.TryGetValue(eventName, out EventCommand eventCommand))
            return eventCommand;

        Debug.LogErrorFormat("Event Command \"{0}\" is not defined in Event Commands list", eventName);
        return null;
    }
}

[Serializable]
public class EventCommand
{
    public string eventName;
    public UnityEvent commandEvent;
}

