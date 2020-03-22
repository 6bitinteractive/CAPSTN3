using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private PlayerStats playerStats;

        [Header("Scene")]
        [SerializeField] private SceneData titleScreen;
        [SerializeField] private SceneData newGameStartingPoint;

        public DayProgression DayProgression => dayProgression;

        public GameData GameData => gameData;
        public bool StartNewGame { get; set; }
        public bool CanLoadSavedData => persistentStorage.HasSaveFile();

        public UnityEvent OnNewGame = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();

            // Ready persistent storage
            persistentStorage.InitializeSavePath();
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
            // Load the TitleScreen
            sceneController.LoadScene(titleScreen);

        }

        public void StartGame()
        {
            // Determine which scene should be loaded
            SceneData sceneToLoad;
            if (!StartNewGame && persistentStorage.HasSaveFile())
            {
                Debug.Log("LOAD SAVED GAME");
                LoadGameData();
                sceneToLoad = gameData.SceneToLoad;
                sceneController.playerStartingPoint = sceneToLoad;
            }
            else
            {
                Debug.Log("START NEW GAME");
                ResetGame();
                OnNewGame.Invoke();
                SaveGameData(); // Overwrite any old data
                sceneToLoad = newGameStartingPoint;
            }

            // Prepare day
            dayProgression.Initialize(questManager);

            // Begin day 1 if it's a new start of the game
            if (sceneToLoad == newGameStartingPoint)
                dayProgression.BeginDay(0);

            // Load the scene
            sceneController.LoadScene(sceneToLoad);
        }

        private void ResetGame()
        {
            GameData.ResetData();
            playerStats.Reset();
            eventManager.ResetEventManager();
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
