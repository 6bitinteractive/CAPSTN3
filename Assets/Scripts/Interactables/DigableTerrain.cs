using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigableTerrain : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject emptyDigableSpotToSpawnPrefab;
    private PoolHandler poolHandler;
    private void Start()
    {
        poolHandler = GetComponent<PoolHandler>();
    }
    public void DisplayInteractability()
    {

    }

    public void Interact(Interactor source, IInteractable target)
    {
        if (!enabled) return;
        poolHandler.SpawnPooledObject(source.GetComponent<Dig>().DigOffset);
    }

    public void ChangeDigableSpotToSpawn()
    {
        for (int i = 0; i < poolHandler.PooledObjectList.Count; i++)
        {
            // Change current active digables into spawning nothing
            Digable digable = poolHandler.PooledObjectList[i].GetComponent<Digable>();
            digable.ObjectToSpawn = null;
            poolHandler.PooledObjectPrefab = emptyDigableSpotToSpawnPrefab;
        }
    }

    public void HideInteractability()
    {

    }
}