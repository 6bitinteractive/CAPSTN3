using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Condition : MonoBehaviour
{
    public Status CurrentStatus => currentStatus;
    public bool Satisfied { get; protected set; }

    public ConditionEvent OnActive = new ConditionEvent();
    public ConditionEvent OnDone = new ConditionEvent();

    private Status currentStatus;

    // TEST
    public ConditionEventType conditionUpdate;
    // ----

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
                    InitializeCondition();
                    OnActive.Invoke(this);

                    // TEST
                    SingletonManager.GetInstance<EventManager>().Trigger<ConditionEvent, Condition>(conditionUpdate, this);
                    // ----

                    break;
                }

            case Status.Evaluating:
                {
                    EvaluateCondition();
                    break;
                }

            case Status.Done:
                {
                    FinalizeCondition();
                    OnDone.Invoke(this);
                    break;
                }
        }
    }

    // Override for inspector use
    public void SwitchStatus(int status)
    {
        SwitchStatus((Status)status);
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
