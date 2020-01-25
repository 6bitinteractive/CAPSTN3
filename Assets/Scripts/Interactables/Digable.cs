﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Digable : MonoBehaviour, IInteractable
{
    public UnityEvent OnDig;
    public UnityEvent OnEndDig;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int ObjectToSpawnJumpSpeed = 300;
    [SerializeField] float DespawnTimer = 10f;
   
    private Collider collider;
    private int currentHp;

    public GameObject ObjectToSpawn { get => objectToSpawn; set => objectToSpawn = value; }

    public void DisplayInteractability()
    {
      
    }

    public void Interact(Interactor source, IInteractable target)
    {
        if (currentHp <= 0) return;
        {
            OnDig.Invoke();
            TakeDamage(source, source.GetComponent<Dig>().DigPower);
        }
    }

    void Awake()
    {
        collider = gameObject.GetComponent<Collider>();
    }

    void OnEnable()
    {
        currentHp = maxHealth;
        collider.enabled = true;   
    }


    private void OnTriggerEnter(Collider collider)
    {
        Dig digSource = collider.GetComponent<Dig>();

        // Player ignores digable  terrain collisions
        if (digSource != null) Physics.IgnoreLayerCollision(8, 9, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        Dig digSource = collider.GetComponent<Dig>();   
        
        if (digSource != null)
        {
            StartCoroutine(DespawnDigable());
            Physics.IgnoreLayerCollision(8, 9, false); // Player can collide with digable terrain
        }
    }
      
    public void TakeDamage(Interactor source, int damageValue)
    {
        currentHp -= damageValue;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);
        if (currentHp <= 0) DugUp(source);
    }

    public void DugUp(Interactor source)
    {
        Dig sourceDig = source.GetComponent<Dig>();
        if (sourceDig != null)
        {
            Physics.IgnoreLayerCollision(8, 9, false); // Player can collide with digable terrain
            collider.enabled = false;
            SpawnObject();
            StartCoroutine(DespawnDigable());
            OnEndDig.Invoke();           
        }
    }

    public void SpawnObject()
    {
        if (ObjectToSpawn != null)
        {
            GameObject newObjectToSpawn = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            newObjectToSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * ObjectToSpawnJumpSpeed); // Make newly spawned object jump up
        }
        else
        {
            //Spawn or display some sort of indicator that there is nothing to dig here
        }
    }

    public IEnumerator DespawnDigable()
    { 
        yield return new WaitForSeconds(DespawnTimer);
        gameObject.SetActive(false);    
    }
}