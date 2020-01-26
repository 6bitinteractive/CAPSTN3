using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConditionEvent : UnityEvent<Condition> { }

public class Condition : MonoBehaviour
{
    public Status CurrentStatus { get; protected set; } // Fix: it's possible to bypass using the SwitchStatus method and simply set this property
    public bool Satisfied { get; protected set; }

    public ConditionEvent OnActive = new ConditionEvent();
    public ConditionEvent OnDone = new ConditionEvent();

    public void SwitchStatus(Status status)
    {
        if (status == CurrentStatus)
            return;

        switch (status)
        {
            case Status.Inactive:
                break;
            case Status.Active:
                InitializeCondition();
                OnActive.Invoke(this);
                break;
            case Status.Evaluating:
                EvaluateCondition();
                break;
            case Status.Done:
                FinalizeCondition();
                OnDone.Invoke(this);
                break;
        }

        CurrentStatus = status;
    }

    protected virtual void InitializeCondition()
    {
        Debug.LogFormat("{0} - Condition initialized.", gameObject.name);
    }

    protected virtual void EvaluateCondition()
    {
        Debug.LogFormat("{0} - Evaluating condition.", gameObject.name);
    }

    protected virtual void FinalizeCondition()
    {
        Debug.LogFormat("{0} - Finalizing condition.", gameObject.name);
    }

    public enum Status
    {
        Inactive,
        Active,
        Evaluating,
        Done
    }
}
