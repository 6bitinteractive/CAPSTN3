using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigableTerrain : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject digableSpotToSpawn;
    [SerializeField] private GameObject emptyDigableSpotToSpawn;
    public void DisplayInteractability()
    {
       
    }

    public void Interact(Interactor source, IInteractable target)
    {
        Instantiate(digableSpotToSpawn, source.transform.position + source.transform.forward, Quaternion.identity);
    }

    public void ChangeDigableSpotToSpawn()
    {
        digableSpotToSpawn = emptyDigableSpotToSpawn;
    }
}