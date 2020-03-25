using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneObjectHandler : Singleton<CrossSceneObjectHandler>
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    // List of cross-scene deliverables
    // OnNewGame
    // |__ loop to list and reset each; bring back to persistent, SetActive(false)

    public GameObject carriedObj { get; set; }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoad;
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
}
