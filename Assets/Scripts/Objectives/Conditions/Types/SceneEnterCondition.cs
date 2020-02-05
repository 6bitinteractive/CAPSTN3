using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnterCondition : Condition
{
    [Header("Condition Requirements")]
    public SceneData requiredSceneToEnter;
    private string sceneLoaded;

    protected override bool RequireSceneLoad => true;

    protected override bool IsSatisfied()
    {
        Debug.Log("SCENE: " + sceneLoaded + " || " + requiredSceneToEnter.SceneName);
        return sceneLoaded == requiredSceneToEnter.SceneName;
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneLoaded = scene.name;
        SwitchStatus(Status.Evaluating);
    }
}
