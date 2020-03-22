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
    private Dictionary<Guid, CutsceneData> cutsceneDataDict = new Dictionary<Guid, CutsceneData>();
    private Dictionary<Guid, DeliverableData> deliverableDataDict = new Dictionary<Guid, DeliverableData>();

    private void AddPersistentData<T>(Dictionary<Guid, T> dictionary, T persistentData) where T : PersistentData
    {
        // Note: Adding via indexer, rather than dictionary.Add(), will automatically add the key if it doesn't exist
        // Otherwise, it'll update the existing entry
        // Reference: https://stackoverflow.com/a/11557431
        dictionary[persistentData.guid] = persistentData;

        //Debug.LogFormat("{0} - Current count: {1}", typeof(T).ToString(), dictionary.Count);
    }

    private T GetPersistentData<T>(Dictionary<Guid, T> dictionary, T persistentData) where T : PersistentData
    {
        //Debug.LogFormat("Finding GUID: {0}", persistentData.guid);
        if (dictionary.TryGetValue(persistentData.guid, out persistentData))
        {
            //Debug.LogFormat("Data Type - {0}", persistentData.GetType());
            return persistentData;
        }

        //Debug.Log("No persistent data found.");
        return null;
    }

    #region AddPersistentData
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

    public void AddPersistentData(CutsceneData cutsceneData)
    {
        AddPersistentData(cutsceneDataDict, cutsceneData);
    }

    public void AddPersistentData(DeliverableData deliverableData)
    {
        AddPersistentData(deliverableDataDict, deliverableData);
    }
    #endregion

    #region GetPersistentData
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

    public CutsceneData GetPersistentData(CutsceneData cutsceneData)
    {
        return GetPersistentData(cutsceneDataDict, cutsceneData);
    }

    public DeliverableData GetPersistentData(DeliverableData deliverableData)
    {
        return GetPersistentData(deliverableDataDict, deliverableData);
    }
    #endregion
}