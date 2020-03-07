﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    private string savePath;

    public void Save(Persistable persistableObject)
    {
        InitializeSavePath();

        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(Persistable persistableObject)
    {
        InitializeSavePath();

        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            persistableObject.Load(new GameDataReader(reader));
        }
    }

    private void InitializeSavePath()
    {
        if (string.IsNullOrWhiteSpace(savePath))
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }
}
