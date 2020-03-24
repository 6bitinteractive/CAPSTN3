using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    [SerializeField] int pooledObjectCount;
    [SerializeField] GameObject pooledObjectPrefab;
    [SerializeField] List<GameObject> pooledObjectList;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 SpawnOffset;

    public List<GameObject> PooledObjectList { get => pooledObjectList; set => pooledObjectList = value; }
    public GameObject PooledObjectPrefab { get => pooledObjectPrefab; set => pooledObjectPrefab = value; }

    private void Start()
    {
        for (int i = 0; i < pooledObjectCount; i++)
        {
            GameObject newPooledObject = Instantiate(PooledObjectPrefab); // Create a pooled object prefab
            PooledObjectList.Add(newPooledObject); // Add pooled object to list
            newPooledObject.transform.parent = parent;
            newPooledObject.transform.localScale = PooledObjectPrefab.transform.localScale;
            newPooledObject.SetActive(false); // Disable pooled object incase its enabled by accident
        }
    }

    public void SpawnPooledObject(GameObject source)
    {
        for (int i = 0; i < PooledObjectList.Count; i++)
        {
            // Check if there arent any active pooled objects
            if (PooledObjectList[i].activeInHierarchy == false)
            {
                PooledObjectList[i].SetActive(true);
                PooledObjectList[i].transform.position = source.transform.position + source.transform.forward + SpawnOffset;
                PooledObjectList[i].transform.rotation = Quaternion.identity;
                break;
            }
            // If the demand of pooled object is getting too high start adding more to the list
            else if (i == PooledObjectList.Count - 1)
            {
                GameObject newPooledObject = Instantiate(PooledObjectPrefab);
                newPooledObject.transform.parent = parent;
                newPooledObject.transform.localScale = PooledObjectPrefab.transform.localScale;
                newPooledObject.SetActive(false);
                PooledObjectList.Add(newPooledObject);
            }
        }
    }
}