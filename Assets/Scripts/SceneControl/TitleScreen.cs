using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button startNewGame, loadGame;

    private GameManager gameManager;
    private SceneController sceneController;

    private IEnumerator Start()
    {
        gameManager = SingletonManager.GetInstance<GameManager>();
        if (!gameManager.CanLoadSavedData)
            loadGame.interactable = false;

        sceneController = SingletonManager.GetInstance<SceneController>();

        // Disable at start
        DisableButtons();

        // Wait until SceneController is ready to load a scene again
        yield return new WaitUntil(() => !sceneController.IsInTransition);

        // Enable when ready
        EnableButtons();
    }

    public void StartNewGame()
    {
        gameManager.StartNewGame = true;
        gameManager.StartGame();

        startNewGame.interactable = loadGame.interactable = false;
    }

    public void LoadGame()
    {
        gameManager.StartNewGame = false;
        gameManager.StartGame();

        startNewGame.interactable = loadGame.interactable = false;
    }

    private void EnableButtons()
    {
        startNewGame.gameObject.SetActive(true);
        //loadGame.gameObject.SetActive(true);
    }

    private void DisableButtons()
    {
        startNewGame.gameObject.SetActive(false);
        loadGame.gameObject.SetActive(false);
    }
}
