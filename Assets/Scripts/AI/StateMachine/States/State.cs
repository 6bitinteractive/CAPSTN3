using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class State : MonoBehaviour
{
    protected Agent agent;
    [SerializeField] private List<Transition> transitions = new List<Transition>();

    void Awake()
    {
        agent = GetComponent<Agent>();
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    public virtual void Update()
    {
        
    }

    public void FixedUpdate()
    {
        foreach (Transition transition in transitions)
        {
            if (transition.aiCondition.CheckCondition(agent))
            {
                transition.targetState.enabled = true; // Enabled component states
                enabled = false; // Disable this state
                return;
            }
        }
    }

    [Serializable]
    public struct Transition
    {
        public AICondition aiCondition;
        public State targetState;
    }
}
