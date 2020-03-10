using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    private string savePath;

    public void Save(Persistable<PersistentData> persistableObject)
    {
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(Persistable<PersistentData> persistableObject)
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            //while (reader.BaseStream.Position != reader.BaseStream.Length) // Check if EndOfFile
            //{
            //
            //}
            try
            {
                persistableObject.Load(new GameDataReader(reader));
            }
            catch (EndOfStreamException)
            {
                Debug.LogError("End of Stream Exception. Try removing the saveFile:\n\n" +
                    "For Windows: <UserDirectory>/AppData/LocalLow/Meowfia/Wander Dog/saveFile\n\n");
            }
        }
    }

    public void InitializeSavePath()
    {
        if (string.IsNullOrWhiteSpace(savePath))
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    public bool HasSaveFile()
    {
        //Debug.Log("Save file exists: " + File.Exists(savePath));
        return File.Exists(savePath);
    }
}
