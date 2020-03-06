using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistable
{
    PersistentData PersistentData { get; set; }

    /// <summary>
    /// Initialize PersistentData and add it to GameData via GameData.AddPersistentData().
    /// </summary>
    void InitializeData();
    void Save(GameDataWriter writer);
    void Load(GameDataReader reader);
}
