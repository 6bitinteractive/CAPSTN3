using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneObjectHandler : Singleton<CrossSceneObjectHandler>
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    public List<CrossSceneObject> crossSceneObjects = new List<CrossSceneObject>();
    public GameObject carriedObj { get; set; }

    private static SceneController sceneController;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        sceneController = sceneController ?? SingletonManager.GetInstance<SceneController>();
        sceneController.BeforePreviousSceneUnload.AddListener(MoveAllToPersistentScene);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoad;
        sceneController.BeforePreviousSceneUnload.AddListener(MoveAllToPersistentScene);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        Mouth mouth = SingletonManager.GetInstance<Mouth>();

        if (mouth == null)
            return;

        if (carriedObj == null)
            return;

        // Mimic biting the object
        carriedObj.GetComponent<CrossSceneObject>().IsCarried = true;
        mouth.CarryObject(carriedObj);
        mouth.GetComponentInParent<Interactor>().CurrentTarget = carriedObj;
        mouth.GetComponentInParent<PlayerController>().Bite();
    }

    private void MoveAllToPersistentScene()
    {
        foreach (var item in crossSceneObjects)
            item.MoveToPersistentScene();
    }
}
