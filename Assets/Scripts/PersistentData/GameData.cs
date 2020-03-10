using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Delete any old saveFile whenever Save/Load's implementations are changed

public partial class GameData : Persistable<PersistentData>
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private DayProgression dayProgression;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private PlayerStats playerStats;

    public SceneData SceneToLoad => Resources.Load<SceneData>
        (string.IsNullOrWhiteSpace(startingPoint)
        ? string.Format("{0}/{1}", sceneDataFolder, currentScene)
        : string.Format("{0}/{1} - {2}", sceneDataFolder, currentScene, startingPoint));

    private string sceneDataFolder = "SceneData";
    private string currentScene, startingPoint;

    public void ResetData()
    {
        currentScene = startingPoint = string.Empty;

        // Clear session data
        persistentDataDict.Clear();
        questEventDataDict.Clear();
        objectiveDataDict.Clear();
        conditionDataDict.Clear();
    }

    // NOTE: Make sure of parity between Save and Load

    public override void Save(GameDataWriter writer)
    {
        //Debug.Log("SAVING............");

        // Current scene
        writer.Write(sceneController.playerStartingPoint.SceneName);
        writer.Write(sceneController.playerStartingPoint.StartingPointName);

        // TODO: Player Stats

        // Current day index
        //writer.Write(dayProgression.CurrentDayIndex);
        dayProgression.Save(writer);

        // Quest-related
        writer.Write(persistentDataDict.Count);
        foreach (var item in persistentDataDict.Keys)
            persistentDataDict[item].Save(writer);

        writer.Write(questEventDataDict.Count);
        foreach (var item in questEventDataDict.Keys)
            questEventDataDict[item].Save(writer);

        writer.Write(objectiveDataDict.Count);
        foreach (var item in objectiveDataDict.Keys)
            objectiveDataDict[item].Save(writer);

        writer.Write(conditionDataDict.Count);
        foreach (var item in conditionDataDict.Keys)
            conditionDataDict[item].Save(writer);

        writer.Write(cutsceneDataDict.Count);
        foreach (var item in cutsceneDataDict.Keys)
            cutsceneDataDict[item].Save(writer);

        // TODO: Zone-specific

        //Debug.Log("SAVING............ DONE");
    }

    public override void Load(GameDataReader reader)
    {
        //Debug.Log("LOADING..........");

        // Scene
        // TODO: need to load this scene and setup SceneController?
        currentScene = reader.ReadString();
        startingPoint = reader.ReadString();

        // TODO: Player Stats

        // Day
        dayProgression.Load(reader);

        // Quest-related - Recreate dictionaries
        int count = reader.ReadInt();
        persistentDataDict = new Dictionary<Guid, PersistentData>(count);

        for (int i = 0; i < count; i++)
        {
            PersistentData p = new PersistentData();
            p.Load(reader);
            persistentDataDict.Add(p.guid, p);
        }

        count = reader.ReadInt();
        questEventDataDict = new Dictionary<Guid, QuestEventData>(count);

        for (int i = 0; i < count; i++)
        {
            QuestEventData q = new QuestEventData();
            q.Load(reader);
            questEventDataDict.Add(q.guid, q);
        }

        count = reader.ReadInt();
        objectiveDataDict = new Dictionary<Guid, ObjectiveData>(count);

        for (int i = 0; i < count; i++)
        {
            ObjectiveData o = new ObjectiveData();
            o.Load(reader);
            objectiveDataDict.Add(o.guid, o);
        }

        count = reader.ReadInt();
        conditionDataDict = new Dictionary<Guid, ConditionData>(count);

        for (int i = 0; i < count; i++)
        {
            ConditionData c = new ConditionData();
            c.Load(reader);
            conditionDataDict.Add(c.guid, c);
        }

        count = reader.ReadInt();
        cutsceneDataDict = new Dictionary<Guid, CutsceneData>(count);

        for (int i = 0; i < count; i++)
        {
            CutsceneData c = new CutsceneData();
            c.Load(reader);
            cutsceneDataDict.Add(c.guid, c);
        }

        // TODO: Zone-specific

        //Debug.Log("LOADING.......... DONE");
    }
}
