using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float attackRadius = 0.2f;

    public float AttackRadius { get => attackRadius; set => attackRadius = value; }

    // Attach AttackEvent to a specific frame within an attack/capture animation
    public void AttackEvent(GameObject source)
    {
        source = gameObject;

        // Set targets to anything that overlaps with sphere
        Collider[] targets = Physics.OverlapSphere(attackPosition.position, AttackRadius, targetMask, QueryTriggerInteraction.Ignore);
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Attackable target = targets[i].GetComponent<Attackable>();    
                Debug.Log("Attacking: " + target.name);
                //Damage logic or capture logic
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, AttackRadius);
    }
}