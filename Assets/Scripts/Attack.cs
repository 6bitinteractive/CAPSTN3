using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float attackRadius = 0.2f;
    [SerializeField] private UnityEvent OnAttack = new UnityEvent();

    private Attackable currentTarget;

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
                currentTarget = target;
                OnAttack.Invoke();
                //Damage logic or capture logic
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, AttackRadius);
    }

    public void DisableTargetMovement()
    {
        Movement movement = currentTarget.GetComponent<Movement>();

        if (movement != null)
            movement.enabled = false;
    }
}