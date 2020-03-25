using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnTargetMoveCondition", menuName = "AI/Conditions/OnTargetMoveCondition")]
public class OnTargetMoveCondition : AICondition
{   public override bool CheckCondition(Agent agent)
    {
        Movement movement = agent.Target.GetComponent<Movement>();

        if (movement != null)
        {
            if (movement.CurrentDirection.z != 0)
                return true;
        }
        return false;
    }
}