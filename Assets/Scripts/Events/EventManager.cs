using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Looking bad... what a convoluted mess...orz

public class EventManager : Singleton<EventManager> // This acts as the mediator among classes, notifying events
{
    private Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();

    /// <summary>
    /// Subscribe to a type of event.
    /// </summary>
    /// <typeparam name="T">The UnityEvent<T></typeparam>
    /// <typeparam name="U">The parameter of the UnityEvent; what is being passed when the event is invoked.</typeparam>
    /// <param name="unityAction">The listener/callback (i.e. the function/method)</param>
    public void Subscribe<T, U>(UnityAction<U> unityAction) where T : UnityEvent<U>
    {
        if (unityAction == null)
        {
            Debug.LogError("Callback is null");
            return;
        }

        var eventType = typeof(T);
        if (subscribers.ContainsKey(eventType))
        {
            subscribers[eventType] = Delegate.Combine(subscribers[eventType], unityAction);
        }
        else
        {
            subscribers.Add(eventType, unityAction);
        }
    }

    public void Unsubscribe<T, U>(UnityAction<U> unityAction) where T : UnityEvent<U>
    {
        if (unityAction == null)
        {
            Debug.LogError("Callback is null");
            return;
        }

        var eventType = typeof(T);
        if (subscribers.ContainsKey(eventType))
        {
            var d = subscribers[eventType];
            d = System.Delegate.Remove(d, unityAction);

            if (d == null)
                subscribers.Remove(eventType);
            else
                subscribers[eventType] = d;
        }
    }

    /// <summary>
    /// Broadcast T event
    /// </summary>
    /// <typeparam name="T">The UnityEvent<T></typeparam>
    /// <typeparam name="U">The parameter of the UnityEvent; what is being passed when the event is invoked.</typeparam>
    /// <param name="argument">What is passed when the event is invoked.</param>
    public void Trigger<T, U>(U argument) where T : UnityEvent<U>
    {
        var eventType = typeof(T);
        if (subscribers.ContainsKey(eventType))
        {
            subscribers[eventType].DynamicInvoke(argument); // Broadcast to any listener of this event type
        }
    }

    // We do this so that whenever we trigger a certain event, we also get to broadcast that that event type has been invoked.
    // This avoids forgetting to tell the EventManager that an event has been invoked.
    public void Trigger<T, U>(EventType<T, U> passedEvent, U argument) where T : UnityEvent<U>
    {
        passedEvent.gameEvent.Invoke(argument); // Broadcast to any listener of this specific event; basically a wrapper?

        Trigger<T, U>(argument);
    }
}

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
#endregion

// Reference
// https://forum.unity.com/threads/looking-for-tips-about-implementation-of-mediator-design-pattern-in-c.299863/
