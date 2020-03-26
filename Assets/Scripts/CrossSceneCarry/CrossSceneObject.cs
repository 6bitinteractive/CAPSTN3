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

    /// <summary>
    /// The scene where this object "exists"
    /// </summary>
    public string LastActiveScene { get; private set; } = null;

    private Biteable biteable;

    private static GameManager gameManager;
    private static CrossSceneObjectHandler crossSceneObjectHandler;
    private Transform thisTransform;
    private StructTransform originalTransform;
    public Rigidbody Rigidbody { get; set; }
    private bool originalIsKinematic;
    private Model model;
    private bool originalIsVisible;

    private void Awake()
    {
        biteable = GetComponent<Biteable>();

        // Store original Transform
        thisTransform = gameObject.transform;
        originalTransform = new StructTransform() { position = thisTransform.position, rotation = thisTransform.rotation, scale = thisTransform.lossyScale };

        // Store originalValue of isKinematic
        Rigidbody = GetComponent<Rigidbody>();
        if (Rigidbody != null) originalIsKinematic = Rigidbody.isKinematic;

        // If it has a model
        model = GetComponentInChildren<Model>();
        if (model != null) originalIsVisible = TransformUtils.IsModelActive(model);
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
        LastActiveScene = SceneManager.GetActiveScene().name;
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
        IsCarried = false;
        thisTransform.position = originalTransform.position;
        thisTransform.rotation = originalTransform.rotation;
        thisTransform.localScale = originalTransform.scale;
        Rigidbody.isKinematic = originalIsKinematic;

        if (model != null)
        {
            TransformUtils.SetActiveRecursively(model.gameObject, originalIsVisible);
            model.gameObject.SetActive(true); // Always make the parent visible
        }
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
