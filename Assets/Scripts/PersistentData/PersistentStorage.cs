using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    public void Save(Persistable persistableObject)
    {
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(Persistable persistableObject)
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            persistableObject.Load(new GameDataReader(reader));
        }
    }
}
