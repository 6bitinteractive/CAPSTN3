using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [Header("Scenes")]
    [SerializeField] private SceneData initialSceneToLoad;
    [SerializeField] private SceneData persistentSceneData;

    [Header("Starting Point")]
    public SceneData playerStartingPoint;

    public static UnityEvent BeforeSceneUnload = new UnityEvent();
    public static UnityEvent AfterSceneLoad = new UnityEvent();

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

    private void Start()
    {
        // Load the very first scene, typically the Title screen
        LoadScene(initialSceneToLoad);
    }

    public void LoadScene(SceneData sceneData)
    {
        StartCoroutine(LoadAndSetActive(sceneData.SceneName));
    }

    // This assumes there's only one additively loaded scene
    private IEnumerator LoadAndSetActive(string sceneName)
    {
#if UNITY_EDITOR // Avoid loading the same scene twice in the Unity Editor
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            Debug.LogFormat("The scene, {0}, is already loaded.", sceneName);
            SceneManager.SetActiveScene(scene); // Set it as the active scene, the one to be unloaded next
            yield break;
        }
#endif

        // Start loading screen/effect here
        // ...

        // Let any listener to this event do their thing
        BeforeSceneUnload.Invoke();

        // Get a reference to scene to be unloaded, which is the current active scene
        Scene sceneToBeUnloaded = SceneManager.GetActiveScene();

        // Unload the scene now, unless it's the persistent scene
        if (sceneToBeUnloaded.name != persistentSceneData.name)
            yield return SceneManager.UnloadSceneAsync(sceneToBeUnloaded);

        // Allow the given scene to load over several frames and add it to the already loaded scenes
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Find the scene that was most recently loaded (the one at the last index of the laoded scenes)
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene
        // This also marks it as the one to be unloaded next
        SceneManager.SetActiveScene(newlyLoadedScene);

        // Let any listener to this event do their thing
        AfterSceneLoad.Invoke();
    }
}
