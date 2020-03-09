using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistable<T>
{
    /// <summary>
    /// Initialize PersistentData and add it to GameData via GameData.AddPersistentData().
    /// </summary>
    void InitializeData();
    T GetPersistentData();
    void SetFromPersistentData();
    void UpdatePersistentData();

    void Save(GameDataWriter writer);
    void Load(GameDataReader reader);
}
