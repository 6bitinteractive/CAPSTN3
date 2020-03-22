using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Condition))]

public class ConditionStateController : MonoBehaviour
{
    [SerializeField] private bool activeAtStart = true;

    private Condition condition;

    private IEnumerator Start()
    {
        if (!activeAtStart)
            yield break;

        yield return new WaitForEndOfFrame();

        condition = GetComponent<Condition>();
        condition.InitializeData();

        if (condition.CurrentStatus != Condition.Status.Done)
            condition.SwitchStatus(Condition.Status.Active);
    }
}
