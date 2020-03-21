using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

public class QuestEvent : Persistable<QuestEventData>
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
    //private QuestEventData questEventData;
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

        Id = GetComponent<GuidComponent>().GetGuid().ToString();
        DisplayName = displayName;
        Description = description;
        currentStatus = Status.Inactive;

        InitializeData();

        foreach (var objective in objectives)
            objective.OnDone.AddListener(EvaluateQuestEvent);
    }

    public void SwitchStatus(Status status)
    {
        if (status == currentStatus)
            return;

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
                    //gameObject.SetActive(false);
                    UpdatePersistentData();

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

    public override QuestEventData GetPersistentData()
    {
        // Note: We do this here so that the correct type is used
        // Calling this at base Persistable class always defaults to getting from the plain PersistentData dictionary
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        currentStatus = Data.status;
        gameObject.SetActive(Data.active);

        //Debug.LogFormat("SET_FROM PersistentData: Active: {0} | State: {1} - {2}" ,Data.active, Data.status, Data.guid);
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.active = gameObject.activeInHierarchy;
        Data.status = currentStatus;

        //Debug.LogFormat("ADD/UPDATE PersistentData: Active: {0} | State: {1} - {2}" ,Data.active, Data.status, Data.guid);

        // Note: We do this here so that the correct type is used
        // Calling this at base Persistable class always defaults to adding to the plain PersistentData dictionary
        gameManager.GameData.AddPersistentData(Data);
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
        Objective incompleteObjective = objectives.Find(x => !x.Complete);
        incompleteObjective?.Activate();
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
