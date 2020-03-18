using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// When you want to create event types that can still show up in the inspector:
#region Event Types
public class EventType<T, U> where T : UnityEvent<U> // We do this to still have access to setting up events via the inspector
{
    public T gameEvent;
}

// These are necessary for EventType to show up in the inspector

[Serializable]
public class QuestEventType : EventType<GameQuestEvent, QuestEvent> { }

[Serializable]
public class ConditionEventType : EventType<ConditionEvent, Condition> { }

[Serializable]
public class InteractionEventType : EventType<InteractionEvent, InteractionData> { }

[Serializable]
public class CutsceneEventType : EventType<CutsceneEvent, Cutscene> { }
#endregion

// When you want to know which events are available for <T, U>:
#region UnityEvent<T>s
[Serializable]
public class GameQuestEvent : UnityEvent<QuestEvent> { }

[Serializable]
public class ObjectiveEvent : UnityEvent<Objective> { }

[Serializable]
public class ConditionEvent : UnityEvent<Condition> { }

[Serializable]
public class InteractionEvent : UnityEvent<InteractionData> { }

public class InteractionData
{
    public Interactor source;
    public GameObject target;
    public InteractionType interactionType;
}

public enum InteractionType
{
    Bark,
    Talk,
    Dig
}

[Serializable]
public class PickupEvent : UnityEvent<PickupData> { }

public class PickupData
{
    public Interactor source;
    public Pickupable pickupable;
    public Type type;

    public enum Type
    {
        Pickup,
        Drop
    }
}

[Serializable]
public class PullEvent : UnityEvent<PullData> { }

public class PullData
{
    public Interactor source;
    public Pullable pullable;
    public Type type;

    public enum Type
    {
        Pull,
        Release
    }
}

[Serializable]
public class ScentModeEvent : UnityEvent<ScentModeData> { }

public class ScentModeData
{
    public Sniffable sniffable;
    public State state;

    public enum State
    {
        Off,
        On
    }
}

[Serializable]
public class DeliveryEvent : UnityEvent<Deliverable> { }

[Serializable]
public class LocationEvent : UnityEvent<LocationData> { }

[Serializable]
public class LocationData
{
    public Location location;
    public GameObject objectInLocation;
    public Type type;

    public enum Type
    {
        Enter,
        Exit
    }
}

[Serializable]
public class DigableEvent : UnityEvent<Digable> { }

[Serializable]
public class CutsceneEvent : UnityEvent<Cutscene> { }

[Serializable]
public class SwitchStateEvent : UnityEvent<State> { }

[Serializable]
public class HungerEvent : UnityEvent<Hunger> { }

[Serializable]
public class GoodBoyPointsEvent : UnityEvent<GoodBoyPoints> { }

[Serializable]
public class GoodBoyPointsHandlerEvent : UnityEvent<GoodBoyPointsHandler> { }
#endregion
