using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Persistable
{
    [SerializeField] private PersistentStorage persistentStorage;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private DayProgression dayProgression;
    [SerializeField] private QuestManager questManager;

    private string currentScene;

    public void SaveGameData()
    {
        persistentStorage.Save(this);
    }

    public void LoadGameData()
    {
        persistentStorage.Load(this);
    }

    public override void Save(GameDataWriter writer)
    {
        // Current day index
        writer.Write(dayProgression.CurrentDayIndex);

        // Quests
        foreach (var quest in questManager.quests)
            quest.Save(writer);

        // Current scene
        writer.Write(sceneController.playerStartingPoint.SceneName);

        // TODO: Zones
    }

    public override void Load(GameDataReader reader)
    {
        // Day
        dayProgression.CurrentDayIndex = reader.ReadInt();

        // Quests
        foreach (var quest in questManager.quests)
            quest.Load(reader);

        // Scene
        // TODO: need to load this scene and setup SceneController?
        currentScene = reader.ReadString();

        // TODO: Zones
    }
}
