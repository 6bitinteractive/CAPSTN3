using Meowfia.WanderDog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Biteable))]

public class CrossSceneObject : MonoBehaviour
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    public bool IsCarried { get; set; }

    private Biteable biteable;

    private static GameManager gameManager;
    private static CrossSceneObjectHandler crossSceneObjectHandler;
    private StructTransform originalTransform;
    private Transform thisTransform;

    private void Awake()
    {
        biteable = GetComponent<Biteable>();

        // Store original Transform
        thisTransform = gameObject.transform;
        originalTransform = new StructTransform() { position = thisTransform.position, rotation = thisTransform.rotation, scale = thisTransform.lossyScale };
    }

    private void Start()
    {
        gameManager = gameManager ?? SingletonManager.GetInstance<GameManager>();
        gameManager.OnNewGame.AddListener(ResetObject);

        crossSceneObjectHandler = crossSceneObjectHandler ?? SingletonManager.GetInstance<CrossSceneObjectHandler>();
        crossSceneObjectHandler.CrossSceneObjects.Add(this);

        biteable.OnBite.AddListener(OnCarried);
        biteable.OnRelease.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
            gameManager.OnNewGame.RemoveListener(ResetObject);

        crossSceneObjectHandler.CrossSceneObjects.Remove(this);

        if (biteable == null) return;
        biteable.OnBite.RemoveListener(OnCarried);
        biteable.OnRelease.RemoveListener(OnReleased);
    }

    private void OnCarried()
    {
        IsCarried = true;
    }

    private void OnReleased()
    {
        IsCarried = false;
        MoveToPersistentScene();
    }

    public void MoveToPersistentScene()
    {
        gameObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(persistentDeliverable.SceneName));
    }

    public void MoveToCurrentActiveScene()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(SceneManager.GetActiveScene().name));
    }

    private void ResetObject()
    {
        thisTransform.position = originalTransform.position;
        thisTransform.rotation = originalTransform.rotation;
        thisTransform.localScale = originalTransform.scale;
    }

    private struct StructTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public StructTransform(Vector3 p, Quaternion q, Vector3 s)
        {
            position = p;
            rotation = q;
            scale = s;
        }
    }
}
