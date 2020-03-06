using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersistentData
{
    public Guid guid;
    public bool active;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;

    public virtual void Save(GameDataWriter writer)
    {
        writer.Write(guid);
        writer.Write(active);
        writer.Write(position);
        writer.Write(rotation);
        writer.Write(scale);
    }

    public virtual void Load(GameDataReader reader)
    {
        guid = reader.ReadGuid();
        active = reader.ReadBool();
        position = reader.ReadVector3();
        rotation = reader.ReadQuaternion();
        scale = reader.ReadVector3();
    }
}

#region Quest-related
[Serializable]
public class QuestEventData : PersistentData
{
    public QuestEvent.Status status;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        writer.Write(Enum.GetName(typeof(QuestEvent.Status), status));
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        Enum.TryParse(reader.ReadString(), out status);
    }
}

[Serializable]
public class ObjectiveData : PersistentData
{
    public bool complete;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        writer.Write(complete);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        complete = reader.ReadBool();
    }
}

[Serializable]
public class ConditionData : PersistentData
{
    public Condition.Status status;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        writer.Write(Enum.GetName(typeof(Condition.Status), status));
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        Enum.TryParse(reader.ReadString(), out status);
    }
}
#endregion