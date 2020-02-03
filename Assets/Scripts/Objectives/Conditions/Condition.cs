using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Condition : MonoBehaviour
{
    public Status CurrentStatus => currentStatus;
    public bool Satisfied { get; protected set; }

    // TODO: Change to EventType<>
    public ConditionEvent OnActive = new ConditionEvent();
    public ConditionEvent OnDone = new ConditionEvent();

    // TEST
    public ConditionEventType conditionUpdate;
    // ----

    protected static EventManager eventManager;
    private bool initialized;
    private Status currentStatus;

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
                    if (!initialized)
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

    // Overload for inspector use
    public void SwitchStatus(int status)
    {
        SwitchStatus((Status)status);
    }

    // NOTE: Only called once
    protected virtual void InitializeCondition()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();

        initialized = true;
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
