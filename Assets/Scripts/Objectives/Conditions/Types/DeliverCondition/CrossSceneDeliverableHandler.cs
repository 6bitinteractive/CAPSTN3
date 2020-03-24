using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossSceneDeliverableHandler : Singleton<CrossSceneDeliverableHandler>
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    private Mouth mouth;

    public GameObject DeliverableObj { get; set; }

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
        mouth = SingletonManager.GetInstance<Mouth>();

        if (mouth == null)
            return;

        if (DeliverableObj == null)
            return;

        // Mimic biting the object
        mouth.GetComponentInParent<Interactor>().CurrentTarget = DeliverableObj;
        mouth.GetComponentInParent<PlayerController>().Bite();
    }
}
