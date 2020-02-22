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
        yield return new WaitForEndOfFrame();

        if (!activeAtStart)
            yield break;

        condition = GetComponent<Condition>();
        condition.SwitchStatus(Condition.Status.Active);
    }
}
