using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

public class QuestEvent : Persistable
{
    [SerializeField] private string displayName;
    [SerializeField] private string description;

    private List<Objective> objectives = new List<Objective>();

    public QuestEventType OnActive;
    public QuestEventType OnDone;

    public string Id { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
    public Status CurrentStatus => currentStatus;

    private static EventManager eventManager;
    private Status currentStatus;

    [HideInInspector] public int order = -1; // We start with -1 to easily determine that the order has not yet been set
    [HideInInspector] public List<QuestPath> pathList = new List<QuestPath>();

    private void Awake()
    {
        // Get children ASAP so they can be saved/loaded
        // Get all the attached Objective componenets; Check if there are any Objective script attached to the gameObject itself
        objectives.AddRange(GetComponents<Objective>());

        if (objectives.Count > 0)
            Debug.LogError("The objective script/s is/are expected to be a child of " + gameObject.name);
        else
            objectives.AddRange(GetComponentsInChildren<Objective>());

        //Debug.Log("Objective count: " + objectives.Count);
    }

    public void Initialize()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();

        Id = Guid.NewGuid().ToString();
        DisplayName = displayName;
        Description = description;
        currentStatus = Status.Inactive;

        foreach (var objective in objectives)
            objective.OnDone.AddListener(EvaluateQuestEvent);
    }

    public void SwitchStatus(Status status)
    {
        currentStatus = status;

        switch (status)
        {
            case Status.Inactive:
                {
                    break;
                }

            case Status.Active:
                {
                    Debug.LogFormat("Activating QuestEvent \"{0}\".", displayName);
                    ActivateCondition();

                    eventManager.Trigger<GameQuestEvent, QuestEvent>(OnActive, this);
                    break;
                }

            case Status.Done:
                {
                    Debug.LogFormat("QuestEvent \"{0}\" complete.", displayName);

                    eventManager.Trigger<GameQuestEvent, QuestEvent>(OnDone, this);
                    break;
                }
        }
    }

    // Overload for inspector use
    public void SwitchStatus(int status)
    {
        SwitchStatus((Status)status);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        Debug.Log("SAVED: " + gameObject.name + " - " + Enum.GetName(typeof(Status), currentStatus));

        // Status
        writer.Write(Enum.GetName(typeof(Status), currentStatus));

        // Save objectives' states
        foreach (var objective in objectives)
            objective.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        // Status
        if (!Enum.TryParse(reader.ReadString(), out currentStatus))
        {
            Debug.Log("Could not parse enum - " + gameObject.name + " - " + currentStatus);
        }
        else
        {
            Debug.Log("Succesfully parsed enum - " + gameObject.name + " - " + currentStatus);
        }

        // Load objectives' states
        foreach (var objective in objectives)
            objective.Load(reader);
    }

    // NOTE: This is mainly used for debugging!
    public void ForceComplete()
    {
        foreach (var objective in objectives)
            objective.ForceComplete();

        currentStatus = Status.Done;
    }

    private void ActivateCondition()
    {
        objectives[0].Activate();
    }

    private void EvaluateQuestEvent(Objective objective)
    {
        // Remove listener
        objective.OnDone.RemoveListener(EvaluateQuestEvent);

        // If there's any objective that has not yet been completed...
        if (objectives.Exists((x) => !x.Complete))
        {
            int index = objectives.FindIndex(x => x == objective);
            index++; // Move to next objective
            if (index < objectives.Count)
                objectives[index].Activate();

            return;
        }

        SwitchStatus(Status.Done);
    }

    public enum Status
    {
        Inactive,
        Active,
        Done
    }
}
