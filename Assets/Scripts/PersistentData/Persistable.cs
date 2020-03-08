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

public class Persistable : MonoBehaviour, IPersistable
{
    public PersistentData PersistentData { get; set; }

    protected static GameManager gameManager;

    public virtual void InitializeData()
    {
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
    }

    public virtual void Save(GameDataWriter writer)
    {
        PersistentData.Save(writer);
    }

    public virtual void Load(GameDataReader reader)
    {
        PersistentData.Load(reader);
    }
}

// Reference: https://catlikecoding.com/unity/tutorials/object-management/
