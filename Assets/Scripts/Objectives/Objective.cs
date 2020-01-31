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

    // TODO: Change to EventType<>
    public ObjectiveEvent OnDone = new ObjectiveEvent();

    //private SequenceType sequenceType = SequenceType.Parallel;

    // TEST
     public void Test(Condition condition)
    {
        if (conditions.Exists(x => condition))
            Debug.Log("Found matching condition. " + condition);
        else
            Debug.Log("No matching condition found. " + condition);
    }
    // ----

    public void Activate()
    {
        // TEST
        SingletonManager.GetInstance<EventManager>().Subscribe<ConditionEvent, Condition>(Test);
        // ----

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

            condition.OnDone.AddListener(EvaluateObjective);
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

        OnDone.Invoke(this);

        condition.OnDone.RemoveListener(EvaluateObjective);

        // TEST
        SingletonManager.GetInstance<EventManager>().Unsubscribe<ConditionEvent, Condition>(Test);
        // ----
    }

    public enum SequenceType
    {
        Parallel,   // All conditions are active at once
        Sequential, // Only one condition is active at a time
    }
}
