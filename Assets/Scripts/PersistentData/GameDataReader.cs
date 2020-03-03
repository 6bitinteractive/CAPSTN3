using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// This class acts as a wrapper for BinaryReader
public class GameDataReader
{
    private BinaryReader reader;

    public GameDataReader(BinaryReader reader)
    {
        this.reader = reader;
    }

    public int ReadInt()
    {
        return reader.ReadInt32(); // Int32 = int; 32 = size of integer (4 bytes = 32 bits)
    }

    public float ReadFloat()
    {
        return reader.ReadSingle(); // ReadSingle = single-precision float
    }

    public bool ReadBool()
    {
        return reader.ReadBoolean();
    }

    public string ReadString()
    {
        return reader.ReadString();
    }

    public Vector3 ReadVector3()
    {
        Vector3 value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        return value;
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        value.w = reader.ReadSingle();
        return value;
    }
}
