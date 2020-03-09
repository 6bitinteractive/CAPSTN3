using Meowfia.WanderDog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public virtual void InitializeData()
    {
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
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
    { }

    // Don't forget to add/update the game data;
    // Use ` gameManager.GameData.AddPersistentData(Data);` at the end
    public virtual void UpdatePersistentData()
    {
        if (Data == null)
            Data = new T { guid = guidComponent.GetGuid() };
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
