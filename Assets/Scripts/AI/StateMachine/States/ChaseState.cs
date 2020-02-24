using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private GameObject sightedIndicator;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float roationSpeed = 10f;
    Attack attack;

    public void Start()
    {
        attack = GetComponent<Attack>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        if (sightedIndicator)
            sightedIndicator.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (sightedIndicator)
            sightedIndicator.SetActive(false);
    }

    public override void Update()
    {
        if (agent.Target == null) return;
        RotateTowardsTarget(agent.Target.transform);
        transform.position = Vector3.MoveTowards(transform.position, agent.Target.transform.position, chaseSpeed * Time.deltaTime);
    }

    private void RotateTowardsTarget(Transform currentTarget)
    {
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, roationSpeed * Time.deltaTime);
    }
}
