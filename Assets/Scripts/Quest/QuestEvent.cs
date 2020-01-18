using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class QuestEvent
{
    [SerializeField] private string displayName;
    [SerializeField] private string description;

    public string Id { get; protected set; } // TODO: use byte // A unique ID allows us to uniquely identify quests even if they have the same names
    public string DisplayName { get; protected set; }
    public string Description { get; protected set; }
    public Status CurrentStatus { get; set; }
    public List<QuestPath> pathList = new List<QuestPath>();

    // TODO: Add Condition (a condition is similar to CAPSTN2 in architecture?)
    // TODO: Add Result/Reward associated with this event
    // TODO: Add UnityEvents

    /// <summary>
    /// The class constructor.
    /// </summary>
    /// <param name="name">The name of the event. The name defined in the inspector takes precedence over what is defined here in the constructor.</param>
    /// <param name="description">The description of the event. The description defined in the inspector takes precedence over what is defined here in the constructor.</param>
    public QuestEvent(string name, string description)
    {
        Id = Guid.NewGuid().ToString();
        DisplayName = string.IsNullOrWhiteSpace(displayName) ? name : displayName;
        Description = string.IsNullOrWhiteSpace(this.description) ? description : this.description;
        CurrentStatus = Status.Inactive;
    }

    public enum Status
    {
        Inactive,
        Active,
        Done
    }
}
