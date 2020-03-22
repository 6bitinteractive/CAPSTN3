using Meowfia.WanderDog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Let the object (with this component attached) save/load whatever it wants to save/load
// In this case, by default the object simply saves values from the Transform component and if it is activeInHierarchy
// Order matters! Save and Load should match; in this case, the order is position, rotation, then scale---i.e. we read in the same order as what we wrote.

[DisallowMultipleComponent] // Make sure only one component per object is attached
[RequireComponent(typeof(GuidComponent))]

public class Persistable<T> : MonoBehaviour, IPersistable<T> where T : PersistentData, new()
{
    public T Data { get; set; } = new T();

    protected static GameManager gameManager;
    protected GuidComponent guidComponent;
    protected bool activeObject = true; // Make sure to set this on subclasses that relies on knowing if its object should be active or not

    private void OnDestroy()
    {
        gameManager.OnNewGame.RemoveListener(ResetData);
    }

    // This is a "hack" since we can't differentiate an On[En/Dis]able called by game logic vs
    // On[En/Dis]able called at start or when the object's about to be destroyed
    public virtual void Enable()
    {
        activeObject = true;
        gameObject.SetActive(true);
        UpdatePersistentData();
    }

    public virtual void Disable()
    {
        activeObject = false;
        UpdatePersistentData();
        gameObject.SetActive(false);
    }

    public virtual void InitializeData()
    {
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
        gameManager.OnNewGame.AddListener(ResetData);
        guidComponent = guidComponent ?? GetComponent<GuidComponent>();

        Data.guid = guidComponent.GetGuid();

        // Check if there's a saved data
        Data = GetPersistentData();

        if (Data != null)
            SetFromPersistentData();
        else
            UpdatePersistentData();
    }

    public virtual T GetPersistentData()
    {
        return null;
    }

    public virtual void SetFromPersistentData()
    {
        //Debug.Log("Set from persistent data");
    }

    // Don't forget to add/update the game data;
    // Use ` gameManager.GameData.AddPersistentData(Data);` at the end
    public virtual void UpdatePersistentData()
    {
        if (Data == null)
        {
            Data = new T { guid = guidComponent.GetGuid() };
        }
    }

    public virtual void ResetData()
    {
        // TODO: Fix this mess...?
        Data = new T { guid = guidComponent.GetGuid() }; // Create a new default data
        gameManager.GameData.AddPersistentData(Data); // Add to the persistent data dictionary
        SetFromPersistentData(); // Set back default to object
    }

    public virtual void Save(GameDataWriter writer)
    {
        Data.Save(writer);
    }

    public virtual void Load(GameDataReader reader)
    {
        Data.Load(reader);
    }
}

// Reference: https://catlikecoding.com/unity/tutorials/object-management/
