using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockState : State
{
    [SerializeField] private GameObject emoticon;

    public override void OnEnable()
    {
        base.OnEnable();
        agent.Target = null;
        navMeshAgent.ResetPath(); // Stop moving

        if (emoticon)
            emoticon.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (emoticon)
            emoticon.SetActive(false);
    }

    public void Shock()
    {
        // Reminders for myself
        // Look at target
        // Play shock Animation
    }
}
