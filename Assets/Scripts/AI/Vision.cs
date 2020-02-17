using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))]
public class Vision : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask ignoreLayerMask;

    public LayerMask TargetLayerMask { get => targetLayerMask; set => targetLayerMask = value; }
    private Agent agent;

    private void Awake()
    {
        agent = GetComponentInParent<Agent>();
    }
    private void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == null) return;

        int collisionLayerMask = 1 << other.gameObject.layer;

       // If collides with target layer mask
       if (collisionLayerMask == TargetLayerMask.value)
       {
            //Spawn a raycast
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction);

            // Check if raycast is hitting an ignorable layer mask
            if (Physics.Raycast(ray, out hit, direction.magnitude, ~ignoreLayerMask))
            {
                // Debug.Log(hit.collider.gameObject);
                agent.Target = null;
                return;
            }

            // Check if ray cast is hitting anything
            else if (Physics.Raycast(ray, out hit, direction.magnitude))
            {    
                //  Debug.Log(hit.collider.gameObject);
                agent.Target = target;
                return;
            }
       }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (agent.Target != null && other.gameObject == agent.Target)
            agent.Target = null;
    }
}