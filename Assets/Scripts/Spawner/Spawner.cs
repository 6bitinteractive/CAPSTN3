using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawnInterval = 3.5f;
    [SerializeField] bool isSpawning = true;
    [SerializeField] List<GameObject> pooledObjectList;
    [SerializeField] List<GameObject> spawnPoints;
    private float timer;

    public void Start()
    {
        for (int i = 0; i < pooledObjectList.Count; i++)
        {
            pooledObjectList[i].transform.parent = gameObject.transform;
        }
    }

    public void Update()
    {
        if (isSpawning == false) return;

        timer += Time.deltaTime; // Start wait timer

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0;
        }
    }

    public void Spawn()
    {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Count);

        for (int i = 0; i < pooledObjectList.Count; i++)
        {          
            // Check if there arent any active pooled objects
            if (pooledObjectList[i].activeInHierarchy == false)
            {
                pooledObjectList[i].SetActive(true);
                pooledObjectList[i].transform.position = spawnPoints[randomSpawnPoint].transform.position;
                pooledObjectList[i].transform.rotation = Quaternion.identity;
                break;
            }

            // If the demand of pooled object is getting too high start adding more to the list
            else if (i == pooledObjectList.Count - 1)
            {
                int randomSpawnablePooledObject = Random.Range(0, pooledObjectList.Count);
                GameObject newPooledObject = Instantiate(pooledObjectList[randomSpawnablePooledObject], spawnPoints[randomSpawnPoint].transform.position, Quaternion.identity);
                newPooledObject.transform.parent = gameObject.transform;
                newPooledObject.SetActive(false);
                pooledObjectList.Add(newPooledObject);
            }
        }
    }
}