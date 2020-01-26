using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnterCondition : Condition
{
    [Header("Condition Requirements")]
    public SceneData requiredSceneToEnter;

    protected override void InitializeCondition()
    {
        base.InitializeCondition();
        SceneManager.sceneLoaded += OnSceneLoad;
        Debug.Log("Required scene entered: " + Satisfied);
    }

    protected override void EvaluateCondition()
    {
        base.EvaluateCondition();

        Debug.Log("Required scene entered: " + Satisfied);

        if (Satisfied)
            SwitchStatus(Status.Done);
    }

    protected override void FinalizeCondition()
    {
        base.FinalizeCondition();
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == requiredSceneToEnter.SceneName)
        {
            Satisfied = true;
            SwitchStatus(Status.Evaluating);
        }
    }
}
