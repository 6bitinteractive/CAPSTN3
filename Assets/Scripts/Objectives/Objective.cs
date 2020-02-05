using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class Objective
{
    public string description;
    public List<Condition> conditions;
    public List<Reaction> reactions;
    public bool Complete { get; private set; }

    public ObjectiveEvent OnDone = new ObjectiveEvent();

    //private SequenceType sequenceType = SequenceType.Parallel;

    public void Activate()
    {

        foreach (var condition in conditions)
        {
            //switch (sequenceType)
            //{
            //    case SequenceType.Parallel:
            //        {
            condition.SwitchStatus(Condition.Status.Active); // For now, we assume conditions are being checked all at once
            //            break;
            //        }

            //    case SequenceType.Sequential:
            //        {
            //            conditions[0].SwitchStatus(Condition.Status.Active);
            //            break;
            //        }
            //}

            // Listen to condition updates
            condition.OnDone.gameEvent.AddListener(EvaluateObjective);
        }
    }

    private void EvaluateObjective(Condition condition)
    {
        Debug.Log("Condition : " + condition.name);
        if (conditions.Exists((x) => x.CurrentStatus != Condition.Status.Done))
            return;

        Complete = true;
        ProcessReactions(condition);
    }

    private void ProcessReactions(Condition condition)
    {
        Debug.LogFormat("Processing reaction/s for {0}", description);
        foreach (var reaction in reactions)
        {
            reaction.Execute();
        }

        // Broadcast that this objective is done
        OnDone.Invoke(this);

        // Stop listening to the condition's event
        condition.OnDone.gameEvent.RemoveListener(EvaluateObjective);
    }

    public enum SequenceType
    {
        Parallel,   // All conditions are active at once
        Sequential, // Only one condition is active at a time
    }
}
