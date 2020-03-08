using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meowfia.WanderDog
{
    public class GameManager : Singleton<GameManager>
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
}
