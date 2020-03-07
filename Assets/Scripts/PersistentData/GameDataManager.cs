using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    [SerializeField] private PersistentStorage persistentStorage;
    [SerializeField] private GameData gameData;

    public GameData GameData => gameData;

    protected override void Awake()
    {
        base.Awake();
        LoadGameData();
    }

    public void SaveGameData()
    {
        persistentStorage.Save(gameData);
    }

    public void LoadGameData()
    {
        persistentStorage.Load(gameData);
    }
}
