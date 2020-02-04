﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnterCondition : Condition
{
    [Header("Condition Requirements")]
    public SceneData requiredSceneToEnter;

    protected override bool RequireSceneLoad => true;

    protected override void EvaluateCondition()
    {
        base.EvaluateCondition();

        Debug.Log("Required scene entered: " + Satisfied);

        if (Satisfied)
            SwitchStatus(Status.Done);
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == requiredSceneToEnter.SceneName)
        {
            Satisfied = true;
            SwitchStatus(Status.Evaluating);
        }
    }
}
