using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeCondition", menuName = "AI/Conditions/TimeCondition")]
public class TimeCondition : AICondition
{
    [SerializeField] private float waitTime = 5f;
    private float elaspedTime = 0;

    public override bool CheckCondition(Agent agent)
    {
        elaspedTime += Time.deltaTime;

        if (elaspedTime >= waitTime)
        {
            elaspedTime = 0; // Reset elapsed time
            return true;
        }
        return false;
    }
}