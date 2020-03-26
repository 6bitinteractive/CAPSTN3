using Meowfia.WanderDog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneObjectHandler : Singleton<CrossSceneObjectHandler>
{
    public List<CrossSceneObject> CrossSceneObjects { get; set; } = new List<CrossSceneObject>();
    public GameObject CarriedObj { get; set; }

    private static GameManager gameManager;
    private static SceneController sceneController;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
        gameManager.OnNewGame.AddListener(ResetCrossSceneHandler);

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
        // Carry any item that's being brought to another scene
        CarryCrossSceneObject();

        // Check which objects should be visible in current active scene
        CheckVisibilty(scene.name);
    }

    private void MoveAllToPersistentScene()
    {
        foreach (var item in CrossSceneObjects)
        {
            item.MoveToPersistentScene();

            if (item.Rigidbody != null)
                item.Rigidbody.isKinematic = true; // We make sure it doesn't fall
        }
    }

    private void CarryCrossSceneObject()
    {
        Mouth mouth = SingletonManager.GetInstance<Mouth>();

        if (mouth == null)
            return;

        if (CarriedObj == null)
            return;

        // Mimic biting the object
        CarriedObj.GetComponent<CrossSceneObject>().IsCarried = true;
        mouth.CarryObject(CarriedObj);
        mouth.GetComponentInParent<Interactor>().CurrentTarget = CarriedObj;
        mouth.GetComponentInParent<PlayerController>().Bite();
    }

    private void CheckVisibilty(string sceneName)
    {
        // Enable relevant objects
        List<CrossSceneObject> crossSceneObjects = new List<CrossSceneObject>();

        crossSceneObjects = CrossSceneObjects.FindAll(x => x.LastActiveScene == sceneName);
        if (crossSceneObjects.Count > 0)
        {
            foreach (var item in crossSceneObjects)
            {
                // NOTE: Assumes that only deliverables are cross-scene objects
                Deliverable deliverable = item.GetComponent<Deliverable>();
                if (deliverable == null) return;
                deliverable.Enable();
            }
        }

        // Disable the rest
        foreach (var item in CrossSceneObjects.Except(crossSceneObjects))
        {
            // NOTE: Assumes that only deliverables are cross-scene objects
            Deliverable deliverable = item.GetComponent<Deliverable>();
            if (deliverable == null || item.IsCarried) return;
            deliverable.Disable();
        }

    }

    private void ResetCrossSceneHandler()
    {
        CarriedObj = null;
    }
}
