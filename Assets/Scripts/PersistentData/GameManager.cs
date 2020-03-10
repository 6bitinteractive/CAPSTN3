using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meowfia.WanderDog
{
    public class GameManager : Singleton<GameManager>
    {
#if UNITY_EDITOR
        [Header("Debug")]
        public bool debug;

        [Space(20f)]
#endif
        // TODO: Lessen Singleton objects by using GameManager as a kind of ServiceLocator???
        [SerializeField] private PersistentStorage persistentStorage;
        [SerializeField] private GameData gameData;
        [SerializeField] private SceneController sceneController;
        [SerializeField] private EventManager eventManager;
        [SerializeField] private DayProgression dayProgression;
        [SerializeField] private QuestManager questManager;

        [Header("Scene")]
        [SerializeField] private SceneData initialSceneToLoad;
        private SceneData sceneToLoad;

        public GameData GameData => gameData;
        public bool StartNewGame { get; set; }
        public bool CanLoadSavedData => persistentStorage.HasSaveFile();

        protected override void Awake()
        {
            base.Awake();

            // Ready persistent storage
            persistentStorage.InitializeSavePath();

            if (!StartNewGame && persistentStorage.HasSaveFile())
            {
                LoadGameData();
                sceneToLoad = gameData.SceneToLoad;
                sceneController.playerStartingPoint = sceneToLoad;
            }
            else
            {
                sceneToLoad = initialSceneToLoad;
            }
        }

        private void OnEnable()
        {
            sceneController.AfterPreviousSceneUnload.AddListener(SaveGameData);
            eventManager.Subscribe<GameQuestEvent, QuestEvent>(OnQuestUpdate);
            eventManager.Subscribe<ObjectiveEvent, Objective>(OnObjectiveUpdate);
            eventManager.Subscribe<ConditionEvent, Condition>(OnConditionUpdate);
            eventManager.Subscribe<CutsceneEvent, Cutscene>(OnCutsceneUpdate);
        }

        private void OnDisable()
        {
            sceneController.AfterPreviousSceneUnload.RemoveListener(SaveGameData);
            eventManager.Unsubscribe<GameQuestEvent, QuestEvent>(OnQuestUpdate);
            eventManager.Unsubscribe<ObjectiveEvent, Objective>(OnObjectiveUpdate);
            eventManager.Unsubscribe<ConditionEvent, Condition>(OnConditionUpdate);
            eventManager.Unsubscribe<CutsceneEvent, Cutscene>(OnCutsceneUpdate);
        }

        private void Start()
        {
            // Load the first scene (usually the TitleScreen)
            sceneController.LoadScene(sceneToLoad);

            // Begin day
            dayProgression.Initialize(questManager);
            dayProgression.BeginDay();
        }

        public void SaveGameData()
        {
#if UNITY_EDITOR
            if (!debug)
#endif
                persistentStorage.Save(gameData);
        }

        public void LoadGameData()
        {
#if UNITY_EDITOR
            if (!debug)
#endif
                persistentStorage.Load(gameData);
        }


        private void OnQuestUpdate(QuestEvent questEvent)
        {
            if (questEvent.CurrentStatus == QuestEvent.Status.Active || questEvent.CurrentStatus == QuestEvent.Status.Done)
                SaveGameData();
        }

        private void OnObjectiveUpdate(Objective objective)
        {
            if (objective.Complete)
                SaveGameData();
        }

        private void OnConditionUpdate(Condition condition)
        {
            if (condition.CurrentStatus == Condition.Status.Done)
                SaveGameData();
        }

        private void OnCutsceneUpdate(Cutscene cutscene)
        {
            if (cutscene.CurrentState == Cutscene.State.Stopped)
                SaveGameData();
        }
    }
}
