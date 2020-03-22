using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is a generic component that can be attached to any object that you want to persist
// Mostly for objects that are in scenes that will be loaded/unloaded throughout the game's session)
// It simply saves attributes found in base PersistentData class

public class BasicPersistableObject : Persistable<PersistentData>
{
    private Transform thisTransform;

    private void Start()
    {
        thisTransform = GetComponent<Transform>();
        InitializeData();
    }

    private void Update()
    {
        if (thisTransform.hasChanged)
        {
            UpdatePersistentData();
            thisTransform.hasChanged = false;
        }
    }

    public override PersistentData GetPersistentData()
    {
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        //Debug.LogFormat("SET: {0} - Pos ({1}) | Rot ({2}) | Scale {3} | Active {4}",
        //    gameObject.name, Data.position, Data.rotation, Data.scale, Data.active);

        thisTransform.position = Data.position;
        thisTransform.rotation = Data.rotation;
        thisTransform.localScale = Data.scale;
        gameObject.SetActive(Data.active);
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.position = thisTransform.position;
        Data.rotation = thisTransform.rotation;
        Data.scale = thisTransform.localScale;
        Data.active = activeObject;

        //Debug.LogFormat("UPDATED: {0} - Pos ({1}) | Rot ({2}) | Scale {3} | Active {4}",
        //    gameObject.name, Data.position, Data.rotation, Data.scale, Data.active);

        gameManager.GameData.AddPersistentData(Data);
    }
}
