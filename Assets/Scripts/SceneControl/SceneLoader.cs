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
        sceneController.playerStartingPoint = sceneDataToLoad;
        sceneController.LoadScene(sceneDataToLoad);
    }
}

