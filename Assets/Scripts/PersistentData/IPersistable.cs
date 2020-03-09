using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This interface is for classes that can't inherit from Persistable, e.g. Singletons
// See Persistable.cs and QuestEvent.cs for sample implementation
public interface IPersistable<T>
{
    T Data { get; set; }

    void InitializeData();
    T GetPersistentData();
    void SetFromPersistentData();
    void UpdatePersistentData();

    void Save(GameDataWriter writer);
    void Load(GameDataReader reader);
}
