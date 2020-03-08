using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistable
{
    /// <summary>
    /// Initialize PersistentData and add it to GameData via GameData.AddPersistentData().
    /// </summary>
    void InitializeData();
    void SetFromPersistentData();
    void UpdatePersistentData();

    void Save(GameDataWriter writer);
    void Load(GameDataReader reader);
}
