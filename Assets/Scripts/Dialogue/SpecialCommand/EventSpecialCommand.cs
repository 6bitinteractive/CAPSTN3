using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpecialCommand : SpecialCommand
{
    EventCommand eventCommandToExecute;

    private static EventCommands eventCommands;

    public EventSpecialCommand(string name)
    {
        eventCommands = eventCommands ?? SingletonManager.GetInstance<EventCommands>();
        eventCommandToExecute = eventCommands.GetEventCommand(name);
    }

    public override void Execute()
    {
        eventCommandToExecute.commandEvent.Invoke();
    }
}
