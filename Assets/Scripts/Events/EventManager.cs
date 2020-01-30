﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Looking bad... what a convoluted mess...orz

public class EventManager : Singleton<EventManager> // This acts as the mediator among classes, notifying events
{
    private Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();

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

    // We do this so that whenever we trigger a certain event, we also get to broadcast that that event type has been invoked.
    // This avoids forgetting to tell the EventManager that an event has been invoked.
    public void Trigger<T, U>(EventType<T, U> passedEvent, U argument) where T : UnityEvent<U>
    {
        passedEvent.gameEvent.Invoke(argument); // Broadcast to any listener of this specific event; basically a wrapper?

        var eventType = typeof(T);
        if (subscribers.ContainsKey(eventType))
        {
            subscribers[eventType].DynamicInvoke(argument); // Broadcast to any listener of this event type
        }
    }
}

public class EventType<T, U> where T : UnityEvent<U> // We do this to still have access to setting up events via the inspector
{
    public T gameEvent;
}

[Serializable]
public class ConditionEventType : EventType<ConditionEvent, Condition> { } // This is necessary for EventType to show up in the inspector

[Serializable]
public class InteractionEventType : EventType<InteractionEvent, InteractionData> { }

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
    public IInteractable target;
    public InteractionType interactionType;
}

public enum InteractionType
{
    Bark,
    Talk
}

// Reference
// https://forum.unity.com/threads/looking-for-tips-about-implementation-of-mediator-design-pattern-in-c.299863/