using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GuidComponent))]

public abstract class Condition : Persistable<ConditionData>
{
    public Status CurrentStatus => currentStatus;
    public bool Satisfied { get; protected set; }

    public ConditionEventType OnActive;
    public ConditionEventType OnDone;

    protected static EventManager eventManager;
    private Status currentStatus;
    private bool initialized;

    private void Start()
    {
        InitializeData();
    }

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
                        InitializeCondition();
                        UpdatePersistentData();
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
                    FinalizeCondition();
                    UpdatePersistentData();
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

    public override ConditionData GetPersistentData()
    {
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        // If the condition is still active, we make sure we call SwitchStatus to fully initialize the condition
        if (Data.status == Status.Active)
            SwitchStatus(Status.Active);
        else
            currentStatus = Data.status;

        Satisfied = Data.satisfied;
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.status = currentStatus;
        Data.satisfied = Satisfied;
        gameManager.GameData.AddPersistentData(Data);
    }

    // NOTE: This is mainly used for debugging!
    public void ForceComplete()
    {
        Satisfied = true;
        currentStatus = Status.Done;
    }

    protected abstract bool IsSatisfied();

    // NOTE: Only called once
    protected virtual void InitializeCondition()
    {
        //Debug.LogFormat("{0} - Condition initialized.", gameObject.name);

        InitializeData();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();

        SceneManager.sceneLoaded += OnSceneLoad;
        GetConditionRequirements(); // We also do this at Initialize for cases where the object is already available at the scene when the QuestEvent activates

        initialized = true;
    }

    protected virtual void EvaluateCondition()
    {
        //Debug.LogFormat("{0} - Evaluating condition.", gameObject.name);

        if (IsSatisfied())
        {
            Satisfied = true;
            SwitchStatus(Status.Done);
        }
        else
        {
            SwitchStatus(Status.Active);
        }
    }

    protected virtual void FinalizeCondition()
    {
        //Debug.LogFormat("{0} - Finalizing condition.", gameObject.name);
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // For cases when a GuidReference can lose its reference when player moves to another scene
    protected virtual void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (Satisfied)
            return;

        GetConditionRequirements();
    }

    protected virtual void GetConditionRequirements()
    { }

    public enum Status
    {
        Inactive,
        Active,
        Evaluating,
        Done
    }
}
