using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionCondition : Condition
{
    [Header("Condition Requirements")]
    [SerializeField] private GuidReference interactionSource;
    [SerializeField] private GuidReference interactionTarget;
    [SerializeField] private InteractionType interactionType;

    private InteractionData condition = new InteractionData();
    private InteractionData interactionToBeEvaluated = new InteractionData();

    private void Start()
    {
        condition.source = interactionSource.gameObject.GetComponent<Interactor>();
        condition.target = interactionTarget.gameObject.GetComponent<IInteractable>();
        condition.interactionType = interactionType;
    }

    protected override bool RequireSceneLoad => true;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        eventManager.Subscribe<InteractionEvent, InteractionData>(GetInteractionData);
    }

    protected override void EvaluateCondition()
    {
        base.EvaluateCondition();

        if (SameInteractionData(condition, interactionToBeEvaluated))
        {
            Debug.Log("Interaction condition satisfied.");
            Satisfied = true;
            SwitchStatus(Status.Done);
        }
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        eventManager.Unsubscribe<InteractionEvent, InteractionData>(GetInteractionData);
    }

    private void GetInteractionData(InteractionData interactionData)
    {
        interactionToBeEvaluated = interactionData;
        SwitchStatus(Status.Evaluating);
    }

    private bool SameInteractionData(InteractionData a, InteractionData b)
    {
        return a.source == b.source
            && a.target == b.target
            && a.interactionType == b.interactionType;
    }
    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
    }
}
