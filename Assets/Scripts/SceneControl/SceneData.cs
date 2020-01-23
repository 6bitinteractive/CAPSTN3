using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneName - StartingPointName")]
public class SceneData : ScriptableObject
{
    [Tooltip("The scene to load; does not necessarily have to share the same name as the ScriptableObject.")]
    [SerializeField] private string sceneName;

    [Tooltip("Leave empty if not applicable.")]
    [SerializeField] private string startingPointName;

    public string SceneName => sceneName;
    public string StartingPointName => startingPointName;
}
