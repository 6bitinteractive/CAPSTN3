﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float waitTime = 2.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private List<Transform> wayPoints;
    private float originalSpeed;
    private float timer;
    private int currentWayPoint = 0;

    private void Start()
    {
        if (wayPoints.Count <= 0)
            Debug.LogError("Please add waypoints to " + gameObject.name + " under (Patrol) script");
    }

    public override void OnEnable()
    {
        base.OnEnable();
       
        originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = patrolSpeed;

        if (animator == null) return;
        animator.SetBool("IsMoving", true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
      
        navMeshAgent.speed = originalSpeed;

        if (animator == null) return;
        animator.SetBool("IsMoving", false);
    }
    public override void Update()
    {
        base.Update();
        Patrol(); 
    }

    public void Patrol()
    {
        RotateTowardsTarget(wayPoints[currentWayPoint], rotationSpeed);
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
                    CheckDespawn();            
                }
                timer = 0;
            }           
        }
    }

    public void CheckDespawn()
    {
        if (gameObject.GetComponent<DeSpawnable>() == null) return;
        gameObject.SetActive(false);
    }
}