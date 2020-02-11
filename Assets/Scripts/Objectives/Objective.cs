﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GuidComponent))]

public class Objective : MonoBehaviour
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

    public void Activate()
    {
        conditions.AddRange(GetComponentsInChildren<Condition>());

        if (conditions.Count == 0)
            Debug.LogWarningFormat("Conditions are expected to be child objects of {0}", gameObject.name);

        foreach (var condition in conditions)
        {
            // Listen to condition updates
            condition.OnDone.gameEvent.AddListener(EvaluateObjective);

            switch (sequenceType)
            {
                case SequenceType.Sequential:
                    {
                        conditions[0].SwitchStatus(Condition.Status.Active); // FIX: This is called more than once?
                        break;
                    }

                case SequenceType.Parallel:
                    {
                        condition.SwitchStatus(Condition.Status.Active);
                        break;
                    }
            }
        }
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
