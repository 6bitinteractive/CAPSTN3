using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtils
{
    /// <summary>
    /// Checks if model object is active---meaning the player will be able to see it
    /// </summary>
    public static bool IsModelActive(Model model)
    {
        if (model == null)
            Debug.LogErrorFormat("{0} Model is null. Make sure to keep the object with Model component active" +
                "and just the actual model/s (its children) as inactive.", model.transform.parent.gameObject.name);

        if (model.transform.childCount == 0)
            Debug.LogErrorFormat("{0} Model has no child objects, i.e. no models set as its children", model.transform.parent.gameObject.name);

        bool modelActive = false;

        if (model)
        {
            for (int i = 0; i < model.transform.childCount; i++)
            {
                modelActive = model.transform.GetChild(i).gameObject.activeSelf;
                if (!modelActive)
                    return false; // When one of its child is inactive, return false
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the parentGameObject and all of its children as active
    /// </summary>
    /// <param name="parentGameObject">Parent gameObject - will be set as active as well</param>
    public static void SetActiveRecursively(GameObject parentGameObject)
    {
        parentGameObject.SetActive(true);
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
            parentGameObject.transform.GetChild(i).gameObject.SetActive(true);
    }
}
