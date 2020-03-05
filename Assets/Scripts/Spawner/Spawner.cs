using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected float spawnInterval = 3.5f;
    [SerializeField] protected bool isSpawning = true;
    [SerializeField] protected List<GameObject> pooledObjectList;
    [SerializeField] protected List<SpawnPoint> spawnPoints;
    [SerializeField] protected List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
    private float timer;
    private int randomSpawnPoint;

    public virtual void Start()
    {
        for (int i = 0; i < pooledObjectList.Count; i++)
        {
            pooledObjectList[i].transform.parent = gameObject.transform;
        }
    }

    public virtual void Update()
    {
        if (isSpawning == false) return;

        timer += Time.deltaTime; // Start wait timer

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0;
        }
    }

    public virtual void Spawn()
    {
        availableSpawnPoints.Clear();
        CheckSpawnPoints();

        // If there are no available spawn points return
        if (availableSpawnPoints.Count.Equals(0)) return;

        for (int i = 0; i < pooledObjectList.Count; i++)
        {
            // Check if there arent any active pooled objects
            if (pooledObjectList[i].activeInHierarchy == false)
            {
                pooledObjectList[i].SetActive(true);
                pooledObjectList[i].transform.position = availableSpawnPoints[randomSpawnPoint].transform.position;
                pooledObjectList[i].transform.rotation = Quaternion.identity;
                break;
            }

            // If the demand of pooled object is getting too high start adding more to the list
            else if (i == pooledObjectList.Count - 1)
            {
                int randomSpawnablePooledObject = Random.Range(0, pooledObjectList.Count);
                GameObject newPooledObject = Instantiate(pooledObjectList[randomSpawnablePooledObject], availableSpawnPoints[randomSpawnPoint].transform.position, Quaternion.identity);
                newPooledObject.transform.parent = gameObject.transform;
                newPooledObject.SetActive(false);
                pooledObjectList.Add(newPooledObject);                  
            }
        }
            
    }

    private void CheckSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            // Check if there arent any active pooled objects
            if (spawnPoints[i].CanSpawn == true)
            {
                availableSpawnPoints.Add(spawnPoints[i]);
                randomSpawnPoint = Random.Range(0, availableSpawnPoints.Count);
            }
        }
        return;
    }
}