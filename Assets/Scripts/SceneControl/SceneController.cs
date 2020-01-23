using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneController : Singleton<SceneController>
{
    [Header("Scenes")]
    [SerializeField] private SceneData initialSceneToLoad;
    [SerializeField] private SceneData persistentSceneData;

    [Header("Starting Point")]
    public SceneData playerStartingPoint;

    public static UnityEvent BeforeSceneUnload = new UnityEvent();
    public static UnityEvent AfterSceneUnload = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();

        // Check if we're at the Persistent scene
        if (gameObject.scene.name != persistentSceneData.SceneName)
        {
            Debug.LogErrorFormat("{0} is expected to live in the {1} scene. It is currently in the {2} scene.",
                GetType().ToString(), persistentSceneData.SceneName, gameObject.scene.name);
        }
    }
}
