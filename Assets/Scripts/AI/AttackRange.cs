using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;

    public LayerMask TargetLayerMask { get => targetLayerMask; set => targetLayerMask = value; }
    private Agent agent;
    private SphereCollider sphereCollider;
    private Attack attack;
    private void Awake()
    {
        agent = GetComponentInParent<Agent>();    
        sphereCollider = GetComponent<SphereCollider>();

        if (attack == null) return;
        {
            attack = GetComponentInParent<Attack>();
            sphereCollider.radius = attack.AttackRadius;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == null) return;

        int collisionLayerMask = 1 << other.gameObject.layer;

        // If collides with target layer mask
        if (collisionLayerMask == TargetLayerMask.value)
        {
            agent.TargetWithinRange = target;
            return;
        }
        return;   
    }

    private void OnTriggerExit(Collider other)
    {
        if (agent.TargetWithinRange != null && other.gameObject == agent.TargetWithinRange)
            agent.TargetWithinRange = null;
    }
}