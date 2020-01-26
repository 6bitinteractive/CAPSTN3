using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    public string Description;
    public List<Condition> conditions;
    public Reaction reaction;

    public bool IsDone { get; private set; }

    private SequenceType sequenceType = SequenceType.Parallel;

    public void Activate()
    {
        foreach (var condition in conditions)
        {
            switch (sequenceType)
            {
                case SequenceType.Parallel:
                    {
                        condition.SwitchStatus(Condition.Status.Active);
                    }
                    break;

                case SequenceType.Sequential:
                    {
                        conditions[0].SwitchStatus(Condition.Status.Active);
                    }
                    break;
            }
        }
    }

    public enum SequenceType
    {
        Parallel,   // All conditions are active at once
        Sequential, // Only one condition is active at a time
    }
}
