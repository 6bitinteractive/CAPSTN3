using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameQuestEvent : UnityEvent<QuestEvent> { }

[RequireComponent(typeof(GuidComponent))]

public class QuestEvent : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] private string description;

    public List<Objective> objectives = new List<Objective>();

    public GameQuestEvent OnActive = new GameQuestEvent();
    public GameQuestEvent OnDone = new GameQuestEvent();

    public string Id { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
    public Status CurrentStatus => currentStatus;

    private Status currentStatus;

    [HideInInspector] public int order = -1; // We start with -1 to easily determine that the order has not yet been set
    [HideInInspector] public List<QuestPath> pathList = new List<QuestPath>();

    public void Initialize()
    {
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
                    ActivateConditions();
                    OnActive.Invoke(this);
                    break;
                }

            case Status.Done:
                {
                    Debug.LogFormat("QuestEvent \"{0}\" complete.", displayName);
                    OnDone.Invoke(this);
                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    // Override for inspector use
    public void SwitchStatus(int status)
    {
        SwitchStatus((Status)status);
    }

    private void ActivateConditions()
    {
        foreach (var objective in objectives)
            objective.Activate();
    }

    private void EvaluateQuestEvent(Objective objective)
    {
        // If there's any objective that has not yet been completed...
        if (objectives.Exists((x) => !x.Complete))
            return;

        // Remove listeners
        foreach (var o in objectives)
            o.OnDone.RemoveListener(EvaluateQuestEvent);

        SwitchStatus(Status.Done);
    }

    public enum Status
    {
        Inactive,
        Active,
        Done
    }
}
