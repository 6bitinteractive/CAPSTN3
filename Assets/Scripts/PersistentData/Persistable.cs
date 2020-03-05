﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let the object (with this component attached) save/load whatever it wants to save/load
// In this case, by default the object simply saves values from the Transform component and if it is activeInHierarchy
// Order matters! Save and Load should match; in this case, the order is position, rotation, then scale---i.e. we read in the same order as what we wrote.

[DisallowMultipleComponent] // Make sure only one component per object is attached

public class Persistable : MonoBehaviour
{
    public virtual void Save(GameDataWriter writer)
    {
        writer.Write(transform.localPosition);
        writer.Write(transform.localRotation);
        writer.Write(transform.localScale);
        writer.Write(gameObject.activeInHierarchy);
    }

    public virtual void Load(GameDataReader reader)
    {
        transform.localPosition = reader.ReadVector3();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
        gameObject.SetActive(reader.ReadBool());
    }
}

// Reference: https://catlikecoding.com/unity/tutorials/object-management/