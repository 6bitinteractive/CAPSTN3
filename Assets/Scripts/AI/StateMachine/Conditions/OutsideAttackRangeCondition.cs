using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutsideAttackRangeCondition", menuName = "AI/Conditions/OutsideAttackRangeCondition")]
public class OutsideAttackRangeCondition : AICondition
{
    public override bool CheckCondition(Agent agent)
    {
        return agent.TargetWithinRange == null;
    }
}