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
    private QuestEventData questEventData;
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

    public override void InitializeData()
    {
        base.InitializeData();

        PersistentData = new QuestEventData();
        questEventData = PersistentData as QuestEventData;
        questEventData.guid = new Guid(Id);

        // Check if there's a saved data
        questEventData = gameDataManager.GameData.GetPersistentData(questEventData);
        if (questEventData == null)
        {
            questEventData.active = gameObject.activeInHierarchy;
            questEventData.status = currentStatus;
            gameDataManager.GameData.AddPersistentData(questEventData);
            Debug.Log("Created new data");
        }
        else
        {
            Debug.LogFormat("Set from saved data - {0} | {1}", questEventData.status, questEventData.active);
            currentStatus = questEventData.status;
            gameObject.SetActive(questEventData.active);
        }
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

        // Update persistent data
        questEventData.active = gameObject.activeInHierarchy;
        questEventData.status = currentStatus;
        gameDataManager.GameData.AddPersistentData(questEventData);
    }

    public enum Status
    {
        Inactive,
        Active,
        Done
    }
}
