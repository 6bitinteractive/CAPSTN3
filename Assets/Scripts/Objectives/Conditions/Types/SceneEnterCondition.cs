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

    protected override void EvaluateCondition()
    {
        base.EvaluateCondition();

        Debug.Log("SCENE: " + sceneLoaded + " || " + requiredSceneToEnter.SceneName);
        if (sceneLoaded == requiredSceneToEnter.SceneName)
            SwitchStatus(Status.Done);
        else
            SwitchStatus(Status.Active);
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        sceneLoaded = scene.name;
        SwitchStatus(Status.Evaluating);
    }
}
