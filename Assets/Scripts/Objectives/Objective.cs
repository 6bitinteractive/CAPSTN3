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

    public bool Complete { get; private set; }

    public ObjectiveEvent OnDone = new ObjectiveEvent();

    // NOTE: For now, we assume all conditions are part of one objective.
    // Ideally, each Objective is it's own object with its own conditions as children.
    private List<Condition> conditions = new List<Condition>();
    //private List<Reaction> reactions = new List<Reaction>();

    private void Awake()
    {
        // Get children ASAP so they can be saved/loaded
        conditions.AddRange(GetComponentsInChildren<Condition>());

        if (conditions.Count == 0)
            Debug.LogErrorFormat("Conditions are expected to be child objects of {0}", gameObject.name);
    }

    public void Activate()
    {
        foreach (var condition in conditions)
        {
            // Listen to condition updates
            condition.OnDone.gameEvent.AddListener(EvaluateObjective);

            // Activate all conditions for parallel sequence type
            if (sequenceType == SequenceType.Parallel)
                condition.SwitchStatus(Condition.Status.Active);
        }

        // For sequential order, only activate the first condition
        if (sequenceType == SequenceType.Sequential)
            conditions[0].SwitchStatus(Condition.Status.Active);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        Debug.Log("SAVED: " + gameObject.name + " - Complete? " + Complete);

        // Complete
        writer.Write(Complete);

        // Save conditions' states
        foreach (var condition in conditions)
            condition.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        // Complete
        Complete = reader.ReadBool();
        Debug.Log("LOADED: " + gameObject.name + " - Complete? " + Complete);

        // Load conditions' states
        foreach (var condition in conditions)
            condition.Load(reader);
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
        //Debug.Log("Condition : " + condition.name);
        if (conditions.Exists((x) => x.CurrentStatus != Condition.Status.Done))
        {
            switch (sequenceType)
            {
                case SequenceType.Sequential:
                    int index = conditions.FindIndex((x) => x == condition);
                    condition.enabled = false;
                    index++; // Move to next condition
                    if (index < conditions.Count)
                        conditions[index].SwitchStatus(Condition.Status.Active);

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

        // Stop listening to the condition's event
        condition.OnDone.gameEvent.RemoveListener(EvaluateObjective);
    }

    public enum SequenceType
    {
        Sequential, // Only one condition is active at a time
        Parallel,   // All conditions are active at once
    }
}
