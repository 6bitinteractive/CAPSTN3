using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersistentData
{
    public Guid guid;
    public bool active = true;
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
        //Debug.LogFormat("QuestEvent Saved Status {0} | {2} - {1}", status, guid, active);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        Enum.TryParse(reader.ReadString(), out status);
        //Debug.LogFormat("QuestEvent Loaded Status {0} | {2} - {1}", status, guid, active);
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

        //Debug.LogFormat("Objective Saved Complete {0} | {2} - {1}", complete, guid, active);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        complete = reader.ReadBool();
        //Debug.LogFormat("Objective Loaded Complete {0} | {2} - {1}", complete, guid, active);
    }
}

[Serializable]
public class ConditionData : PersistentData
{
    public Condition.Status status;
    public bool satisfied;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        writer.Write(Enum.GetName(typeof(Condition.Status), status));
        writer.Write(satisfied);
        //Debug.LogFormat("Condition Saved Status {0} | {2} - {1}", status, guid, active);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        Enum.TryParse(reader.ReadString(), out status);
        satisfied = reader.ReadBool();
        //Debug.LogFormat("Condition Saved Status {0} | {2} - {1}", status, guid, active);
    }
}
#endregion

[Serializable]
public class CutsceneData : PersistentData
{
    public int playCount;
    public Cutscene.State state;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        writer.Write(playCount);
        writer.Write(Enum.GetName(typeof(Cutscene.State), state));
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        playCount = reader.ReadInt();
        Enum.TryParse(reader.ReadString(), out state);
    }
}