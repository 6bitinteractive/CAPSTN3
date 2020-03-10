using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCondition : Condition
{
    [Header("Condition Requirements")]
    [SerializeField] private GuidReference requiredCutscene;

    [Tooltip("Stopped means the whole cutscene has been viewed.\n\nPlaying means the moment the cutscene is played.")]
    [SerializeField] private Cutscene.State requiredState;

    private Cutscene cutsceneRequired, cutsceneToBeEvauated;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<CutsceneEvent, Cutscene>(GetPlayedCutscene);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<CutsceneEvent, Cutscene>(GetPlayedCutscene);
    }

    protected override bool IsSatisfied()
    {
        return cutsceneRequired == cutsceneToBeEvauated && cutsceneToBeEvauated.CurrentState == requiredState;
    }

    private void GetPlayedCutscene(Cutscene cutscene)
    {
        cutsceneToBeEvauated = cutscene;
        SwitchStatus(Status.Evaluating);
    }

    protected override void GetConditionRequirements()
    {
        if (requiredCutscene.gameObject != null)
        {
            cutsceneRequired = requiredCutscene.gameObject.GetComponent<Cutscene>();
        }
    }
}
