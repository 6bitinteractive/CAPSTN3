using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    public void BarkEvent()
    {
        // Set targets to anything that overlaps with sphere
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, -1, QueryTriggerInteraction.Ignore); //Ignore trigger is to prevent being called multiple times on one object 
        {
            // Apply BarkEvent to all targets
            foreach (var target in targets)
            {            
                Barkable barkable = target.GetComponent<Barkable>();
                if (barkable != null)
                {
                    barkable.Interact();
                    Debug.Log("Barking at " + target.gameObject.name);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}