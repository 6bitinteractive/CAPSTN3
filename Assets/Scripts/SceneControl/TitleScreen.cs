using Meowfia.WanderDog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Button startNewGame, loadGame;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = SingletonManager.GetInstance<GameManager>();
        if (!gameManager.CanLoadSavedData)
            loadGame.interactable = false;
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
}
