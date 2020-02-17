using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : State
{
    [SerializeField] private float wanderRadius = 2f;
    [SerializeField] private float wanderSpeed = 1f;
    [SerializeField] private float waitTime = 2.5f;
    private float originalSpeed;
    private float timer;
    public override void OnEnable()
    {
        base.OnEnable();
        originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = wanderSpeed;
        agent.Target = null;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        navMeshAgent.speed = originalSpeed;
    }
    public override void Update()
    {
        base.Update();
        Wander();
    }

    public void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }
}