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

// Reference
// https://forum.unity.com/threads/looking-for-tips-about-implementation-of-mediator-design-pattern-in-c.299863/
