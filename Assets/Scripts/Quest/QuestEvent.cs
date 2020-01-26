using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class QuestGameEvent : UnityEvent<QuestEvent> { }

[Serializable]
public class QuestEvent
{
    [SerializeField] private string displayName;
    [SerializeField] private string description;

    public List<Objective> objectives = new List<Objective>();

    public QuestGameEvent OnActive = new QuestGameEvent();
    public QuestGameEvent OnDone = new QuestGameEvent();

    public string Id { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
    public Status CurrentStatus { get; private set; }

    [HideInInspector] public int order = -1; // We start with -1 to easily determine that the order has not yet been set
    [HideInInspector] public List<QuestPath> pathList = new List<QuestPath>();

    public QuestEvent()
    {
        Id = Guid.NewGuid().ToString();
        DisplayName = displayName;
        Description = description;
        CurrentStatus = Status.Inactive;
    }

    /// <summary>
    /// The class constructor.
    /// </summary>
    /// <param name="name">The name of the event. The name defined in the inspector takes precedence over what is defined here in the constructor.</param>
    /// <param name="description">The description of the event. The description defined in the inspector takes precedence over what is defined here in the constructor.</param>
    //public QuestEvent(string name, string description)
    //{
    //    Id = Guid.NewGuid().ToString();
    //    DisplayName = string.IsNullOrWhiteSpace(displayName) ? name : displayName;
    //    Description = string.IsNullOrWhiteSpace(this.description) ? description : this.description;
    //    CurrentStatus = Status.Inactive;
    //}

    public void SwitchStatus(Status status)
    {
        CurrentStatus = status;

        switch (status)
        {
            case Status.Inactive:
                break;
            case Status.Active:
                ActivateConditions();
                OnActive.Invoke(this);
                Debug.LogFormat("QuestEvent \"{0}\" is now active.", displayName);
                break;
            case Status.Done:
                OnDone.Invoke(this);
                break;
            default:
                break;
        }
    }

    private void ActivateConditions()
    {
        foreach (var objective in objectives)
        {
            objective.Activate();
        }
    }

    public enum Status
    {
        Inactive,
        Active,
        Done
    }
}
