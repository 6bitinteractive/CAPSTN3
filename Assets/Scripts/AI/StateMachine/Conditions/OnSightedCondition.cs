using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OnSightedCondition", menuName = "AI/Conditions/OnSightedCondition")]
public class OnSightedCondition : AICondition
{
    public override bool CheckCondition(Agent agent)
    {
        return agent.Target;
    }
}