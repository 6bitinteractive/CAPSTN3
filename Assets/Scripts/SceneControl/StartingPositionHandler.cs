using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles an object's starting position
public class StartingPositionHandler : MonoBehaviour
{
    private SceneController sceneController;

    private void Start()
    {
        sceneController = SingletonManager.GetInstance<SceneController>();
        SetStartingPosition();
    }

    private void SetStartingPosition()
    {
        if (sceneController.playerStartingPoint == null)
        {
            Debug.LogError("No starting point indicated in scene data.");
            return;
        }

        Transform startingPosition = StartingPosition.FindStartingPosition(sceneController.playerStartingPoint.StartingPointName);
        transform.position = startingPosition.position;
    }
}
