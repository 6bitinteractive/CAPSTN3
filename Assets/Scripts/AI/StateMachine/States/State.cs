using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Agent))]
public class State : MonoBehaviour
{
    protected Agent agent;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    [SerializeField] protected UnityEvent onEnable;
    [SerializeField] protected UnityEvent onDisable; 
    [SerializeField] private List<Transition> transitions = new List<Transition>();

    void Awake()
    {
        agent = GetComponent<Agent>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public virtual void OnEnable()
    {
        onEnable.Invoke();
    }

    public virtual void OnDisable()
    {
        onDisable.Invoke();
    }

    public virtual void Update()
    {
        if (animator == null) return;
        AnimateMovement(navMeshAgent.velocity);
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

    public virtual void RotateTowardsTarget(Transform targetTransform, float rotationSpeed)
    {
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void AnimateMovement(Vector3 direction)
    {
        animator.SetFloat("VelX", direction.x);
        animator.SetFloat("VelY", direction.y);
    }

    [Serializable]
    public struct Transition
    {
        public AICondition aiCondition;
        public State targetState;
    }  
}
