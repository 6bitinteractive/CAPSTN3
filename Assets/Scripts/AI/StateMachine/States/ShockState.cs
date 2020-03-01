using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockState : State
{
    [SerializeField] private GameObject emoticon;

    public override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent.ResetPath(); // Stop moving

        if (emoticon)
            emoticon.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        agent.TargetWithinRange = null;
        if (emoticon)
            emoticon.SetActive(false);
    }

    public override void Update()
    {
        if (agent.TargetWithinRange == null) return;
        base.Update();
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (agent.TargetWithinRange.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2 * Time.deltaTime);
    }
}
