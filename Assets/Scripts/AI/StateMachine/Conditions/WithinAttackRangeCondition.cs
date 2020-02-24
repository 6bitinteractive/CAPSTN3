using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WithinAttackRangeCondition", menuName = "AI/Conditions/WithinAttackRangeCondition")]
public class WithinAttackRangeCondition : AICondition
{   public override bool CheckCondition(Agent agent)
    {
        return agent.TargetWithinRange;
    }
}