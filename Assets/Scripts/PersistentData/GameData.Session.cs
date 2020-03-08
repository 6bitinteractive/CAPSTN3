using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds everything related to handling game data during a session

public partial class GameData
{
    private Dictionary<Guid, PersistentData> persistentDataDict = new Dictionary<Guid, PersistentData>();
    private Dictionary<Guid, QuestEventData> questEventDataDict = new Dictionary<Guid, QuestEventData>();
    private Dictionary<Guid, ObjectiveData> objectiveDataDict = new Dictionary<Guid, ObjectiveData>();
    private Dictionary<Guid, ConditionData> conditionDataDict = new Dictionary<Guid, ConditionData>();

    private void AddPersistentData<T>(Dictionary<Guid, T> dictionary, T persistentData) where T : PersistentData
    {
        // Note: Adding via indexer, rather than dictionary.Add(), will automatically add the key if it doesn't exist
        // Otherwise, it'll update the existing entry
        // Reference: https://stackoverflow.com/a/11557431
        dictionary[persistentData.guid] = persistentData;
        Debug.Log("Current count: " + typeof(T).ToString() + " - " + dictionary.Count);
    }

    private T GetPersistentData<T>(Dictionary<Guid, T> dictionary, T persistentData) where T : PersistentData
    {
        //Debug.LogFormat("Finding GUID: {0}", persistentData.guid);
        if (dictionary.TryGetValue(persistentData.guid, out persistentData))
        {
            return persistentData;
        }

        //Debug.Log("No persistent data found.");
        return null;
    }

    public void AddPersistentData(PersistentData persistentData)
    {
        AddPersistentData(persistentDataDict, persistentData);
    }

    public void AddPersistentData(QuestEventData questEventData)
    {
        AddPersistentData(questEventDataDict, questEventData);
    }

    public void AddPersistentData(ObjectiveData objectiveData)
    {
        AddPersistentData(objectiveDataDict, objectiveData);
    }

    public void AddPersistentData(ConditionData conditionData)
    {
        AddPersistentData(conditionDataDict, conditionData);
    }

    public PersistentData GetPersistentData(PersistentData persistentData)
    {
        return GetPersistentData(persistentDataDict, persistentData);
    }

    public QuestEventData GetPersistentData(QuestEventData questEventData)
    {
        return GetPersistentData(questEventDataDict, questEventData);
    }

    public ObjectiveData GetPersistentData(ObjectiveData objectiveData)
    {
        return GetPersistentData(objectiveDataDict, objectiveData);
    }

    public ConditionData GetPersistentData(ConditionData conditionData)
    {
        return GetPersistentData(conditionDataDict, conditionData);
    }
}