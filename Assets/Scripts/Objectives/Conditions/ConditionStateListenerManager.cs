using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Please don't use this anymore; it's not necessary
// Simply use a QuestEventState or ConditionState with a ConditionStateController

// ==========
// Manages the conditions, mostly listeners for quest event/condition state changes
// This doesn't live in the Persistent scene

public class ConditionStateListenerManager : MonoBehaviour
{
    private List<Condition> conditions = new List<Condition>();

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        conditions.AddRange(GetComponentsInChildren<Condition>());

        foreach (var condition in conditions)
        {
            // All conditions must be active at start
            condition.SwitchStatus(Condition.Status.Active);
        }

        foreach (var condition in conditions)
        {
            // If it's been done, invoke OnActive/OnDone events to apply reactions
            if (condition.CurrentStatus == Condition.Status.Done)
            {
                condition.OnActive.gameEvent.Invoke(condition);
                condition.OnDone.gameEvent.Invoke(condition);
            }
        }
    }
}
