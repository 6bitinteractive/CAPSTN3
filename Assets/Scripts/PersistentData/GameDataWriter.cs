using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// This class acts as a wrapper for BinaryWriter
public class GameDataWriter
{
    private BinaryWriter writer;

    public GameDataWriter(BinaryWriter writer)
    {
        this.writer = writer;
    }

    // Write data to the file using BinaryWriter's Write() method

    public void Write(int value)
    {
        writer.Write(value);
    }

    public void Write(float value)
    {
        writer.Write(value);
    }

    public void Write(bool value)
    {
        writer.Write(value);
    }

    public void Write(string value)
    {
        writer.Write(value);
    }

    public void Write(Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public void Write(Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }
}