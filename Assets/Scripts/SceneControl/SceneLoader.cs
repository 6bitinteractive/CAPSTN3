using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private static SceneController sceneController;

    private void Start()
    {
        sceneController = sceneController ?? SingletonManager.GetInstance<SceneController>();
    }

    public void LoadScene(SceneData sceneDataToLoad)
    {
        // Set the starting position
        if (string.IsNullOrWhiteSpace(sceneDataToLoad.StartingPointName))
        {
            sceneController.playerStartingPoint = null;
            Debug.LogWarning("No starting position at the scene that is about to be loaded.");
        }
        else
        {
            sceneController.playerStartingPoint = sceneDataToLoad;
        }

        sceneController.LoadScene(sceneDataToLoad);
    }
}

