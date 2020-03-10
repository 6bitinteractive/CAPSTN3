using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

public class Objective : Persistable<ObjectiveData>
{
    [Tooltip("Optional. Describe what is expected in this objective.")]
    public string description;

    [Header("Objective Sequence Type")]
    [Tooltip("Sequential = one condition is active at a time.\nParallel = all conditions are active at once.")]
    [SerializeField] private SequenceType sequenceType = SequenceType.Sequential;

    public List<Condition> Conditions => conditions;
    public bool Complete { get; private set; }

    public ObjectiveEvent OnDone = new ObjectiveEvent();

    // NOTE: For now, we assume all conditions are part of one objective.
    // Ideally, each Objective is it's own object with its own conditions as children.
    private List<Condition> conditions = new List<Condition>();
    //private List<Reaction> reactions = new List<Reaction>();

    private void Awake()
    {
        conditions.AddRange(GetComponentsInChildren<Condition>());

        if (conditions.Count == 0)
            Debug.LogErrorFormat("Conditions are expected to be child objects of {0}", gameObject.name);
    }

    private void Start()
    {
        InitializeData();

        //// Make sure conditions have the correct data before checking
        //foreach (var condition in conditions)
        //    condition.InitializeData();

        //// Force check
        //Debug.Log("Last done: " + conditions.FindLast(x => x.CurrentStatus == Condition.Status.Done));
        //EvaluateObjective(conditions.FindLast(x => x.CurrentStatus == Condition.Status.Done));
    }

    public void Activate()
    {
        InitializeData();

        // Make sure conditions have the correct data before checking
        foreach (var condition in conditions)
            condition.InitializeData();

        foreach (var condition in conditions)
        {
            // Listen to condition updates
            condition.OnDone.gameEvent.AddListener(EvaluateObjective);

            // Activate all conditions for parallel sequence type
            if (sequenceType == SequenceType.Parallel)
                condition.SwitchStatus(Condition.Status.Active);
        }

        // For sequential order, only activate the first condition that has not yet been done
        if (sequenceType == SequenceType.Sequential)
        {
            Condition inactiveCondition = conditions.Find(x => x.CurrentStatus != Condition.Status.Done);
            inactiveCondition?.SwitchStatus(Condition.Status.Active);
        }
    }

    public override ObjectiveData GetPersistentData()
    {
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        Complete = Data.complete;
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.complete = Complete;
        gameManager.GameData.AddPersistentData(Data);
    }

    // NOTE: This is mainly used for debugging!
    public void ForceComplete()
    {
        foreach (var condition in conditions)
            condition.ForceComplete();

        Complete = true;
    }

    private void EvaluateObjective(Condition condition)
    {
        if (condition == null)
            return;

        //Debug.Log("Condition : " + condition.name);
        if (conditions.Exists((x) => x.CurrentStatus != Condition.Status.Done))
        {
            switch (sequenceType)
            {
                case SequenceType.Sequential:
                    int index = conditions.FindIndex(x => x == condition);
                    condition.enabled = false;
                    index++; // Move to next condition
                    if (index < conditions.Count)
                    {
                        Debug.Log(conditions[index]);
                        conditions[index].SwitchStatus(Condition.Status.Active);
                    }

                    return;

                case SequenceType.Parallel:
                    return;
            }
        }

        Complete = true;
        ProcessReactions(condition);
    }

    private void ProcessReactions(Condition condition)
    {
        //Debug.LogFormat("Processing reaction/s for {0}", description);
        //foreach (var reaction in reactions)
        //{
        //    reaction.Execute();
        //}

        // Broadcast that this objective is done
        OnDone.Invoke(this);
        UpdatePersistentData();

        // Stop listening to the condition's event
        condition.OnDone.gameEvent.RemoveListener(EvaluateObjective);
    }

    public enum SequenceType
    {
        Sequential, // Only one condition is active at a time
        Parallel,   // All conditions are active at once
    }
}
