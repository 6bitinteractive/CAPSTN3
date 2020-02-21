using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the conditions, mostly listeners for quest event/condition state changes
// This doesn't live in the Persistent scene

public class ConditionStateListenerManager : MonoBehaviour
{
    private List<Condition> conditions = new List<Condition>();

    private void Awake()
    {
        conditions.AddRange(GetComponentsInChildren<Condition>());

        // All conditions must be active at start
        foreach (var condition in conditions)
            condition.SwitchStatus(Condition.Status.Active);
    }
}
