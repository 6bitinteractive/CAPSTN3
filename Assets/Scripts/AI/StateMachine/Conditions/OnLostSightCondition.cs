using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnLostSightCondition", menuName = "AI/Conditions/OnLostSightCondition")]
public class OnLostSightCondition : AICondition
{
    public override bool CheckCondition(Agent agent)
    {
        return agent.Target == null;
    }
}