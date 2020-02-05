using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestEventStateCondition : Condition
{
    [Header("Condition Requirements")]
    [SerializeField] private GuidReference questEvent;
    [SerializeField] private QuestEvent.Status requiredStatus = QuestEvent.Status.Active;

    private QuestEvent requiredQuestEvent;
    private QuestEvent.Status statusToAssess;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        requiredQuestEvent = questEvent.gameObject.GetComponent<QuestEvent>();

        // If the event has already passed
        if (requiredQuestEvent.CurrentStatus != QuestEvent.Status.Inactive)
        {
            statusToAssess = requiredQuestEvent.CurrentStatus;
            SwitchStatus(Status.Evaluating);
        }
        else
        {
            GetConditionRequirements();
        }
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        switch (requiredStatus)
        {
            case QuestEvent.Status.Active:
                requiredQuestEvent.OnActive.gameEvent.RemoveListener(OnQuestUpdate);
                break;
            case QuestEvent.Status.Done:
                requiredQuestEvent.OnDone.gameEvent.RemoveListener(OnQuestUpdate);
                break;
        }
    }

    protected override bool IsSatisfied()
    {
        //Debug.Log("ASSESS: " + statusToAssess + " || " + requiredStatus);
        return statusToAssess == requiredStatus;
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        base.OnSceneLoad(scene, loadSceneMode);
        GetConditionRequirements();
    }

    private void OnQuestUpdate(QuestEvent questEvent)
    {
        statusToAssess = questEvent.CurrentStatus;
        SwitchStatus(Status.Evaluating);
    }

    private void GetConditionRequirements()
    {
        switch (requiredStatus)
        {
            case QuestEvent.Status.Inactive:
                Debug.LogError("Please don't choose Inactive. Only Active and Done states are supported.");
                break;
            case QuestEvent.Status.Active:
                requiredQuestEvent.OnActive.gameEvent.AddListener(OnQuestUpdate);
                break;
            case QuestEvent.Status.Done:
                requiredQuestEvent.OnDone.gameEvent.AddListener(OnQuestUpdate);
                break;
        }
    }
}
