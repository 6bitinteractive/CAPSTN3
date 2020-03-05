using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCollisionCondition : SpawnPoint
{
    [SerializeField] private LayerMask targetLayerMask;

    public override void Start()
    {
        canSpawn = false;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target == null) return;

        int collisionLayerMask = 1 << target.layer;

        // If collides with target layer mask
        if (collisionLayerMask == targetLayerMask.value)
        {
            canSpawn = true;
            return;
        }
        return;
    }

    private void OnTriggerExit(Collider other)
    {
        canSpawn = false;
    }
}
