using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class Condition : MonoBehaviour
{
    public Status CurrentStatus => currentStatus;
    public bool Satisfied { get; protected set; }

    public ConditionEventType OnActive;
    public ConditionEventType OnDone;

    protected static EventManager eventManager;
    protected abstract bool RequireSceneLoad { get; }
    private Status currentStatus;
    private bool initialized;

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
                    {
                        if (RequireSceneLoad)
                            SceneManager.sceneLoaded += OnSceneLoad;

                        InitializeCondition();
                    }

                    eventManager.Trigger<ConditionEvent, Condition>(OnActive, this);
                    break;
                }

            case Status.Evaluating:
                {
                    EvaluateCondition();
                    break;
                }

            case Status.Done:
                {
                    if (RequireSceneLoad)
                    {
                        SceneManager.sceneLoaded -= OnSceneLoad;
                    }

                    FinalizeCondition();
                    eventManager.Trigger<ConditionEvent, Condition>(OnDone, this);
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
        Satisfied = true; // Note: For now, it's assumed that a condition flagged as Done means it also has been satisfied, i.e. no Fail
    }

    // For cases when a GuidReference can lose its reference when player moves to another scene
    protected virtual void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (Satisfied)
            return;
    }

    public enum Status
    {
        Inactive,
        Active,
        Evaluating,
        Done
    }
}
