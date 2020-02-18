using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float waitTime = 2.5f;
    [SerializeField] private List<Transform> wayPoints;
    private float originalSpeed;
    private float timer;
    private int currentWayPoint = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = patrolSpeed;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        navMeshAgent.speed = originalSpeed;
    }
    public override void Update()
    {
        base.Update();
        Patrol();
    }

    public void Patrol()
    {
        navMeshAgent.SetDestination(wayPoints[currentWayPoint].position);

        // Check if near waypoint
        if (Vector3.Distance(transform.position, wayPoints[currentWayPoint].position) <= 0.5f)
        {
            navMeshAgent.ResetPath(); // Stop moving
            timer += Time.deltaTime; // Start wait timer

            if (timer >= waitTime)
            {
                currentWayPoint++; // Move to next waypoint

                // Check if reached final waypoint
                if (currentWayPoint == wayPoints.Count)
                {
                    currentWayPoint = 0; // Reset waypoint target to first waypoint
                }
                timer = 0;
            }           
        }
    }
}
