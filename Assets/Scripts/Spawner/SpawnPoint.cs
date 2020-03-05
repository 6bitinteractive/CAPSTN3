using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private protected bool canSpawn;

    public bool CanSpawn { get => canSpawn; set => canSpawn = value; }

    public virtual void Start()
    {
        canSpawn = true;
    }
}