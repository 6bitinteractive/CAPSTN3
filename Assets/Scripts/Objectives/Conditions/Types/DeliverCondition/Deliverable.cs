using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Sniffable))]

public class Deliverable : Persistable<DeliverableData>
{
    [Tooltip("Scene where crossScene deliverables are stored; by default it is the Persistent scene")]
    [SerializeField] private SceneData persistentDeliverable;

    [SerializeField] private bool crossSceneDeliverable;

    private Biteable biteable;
    private Pickupable pickupable;
    private Outlineable outlineable;
    private Sniffable sniffable;
    private Rigidbody rb;
    private Collider thisCollider;
    private Transform thisTransform;
    private bool activeDeliverable;

    public bool IsCarried { get; set; }
    public bool IsCrossSceneDeliverable { get; private set; }
    public bool Delivered { get; private set; }

    private void Start()
    {
        IsCrossSceneDeliverable = crossSceneDeliverable;

        Init();
        InitializeData();

        if (Delivered)
        {
            // Make sure it's a root object
            gameObject.transform.SetParent(null);

            // Make it visible
            Enable();
        }

        if (activeDeliverable)
            Activate();
    }

    private void Update()
    {
        if (thisTransform.hasChanged)
        {
            UpdatePersistentData();
            thisTransform.hasChanged = false;
        }
    }

    public void Activate()
    {
        Init();
        thisCollider.enabled = true;
        biteable.enabled = true;
        outlineable.enabled = true;
        sniffable.SetCurrentTarget();

        activeDeliverable = true;
        UpdatePersistentData();
    }

    public void OnDeliver()
    {
        if (biteable)
            biteable.enabled = false;

        if (pickupable)
            pickupable.enabled = false;

        if (outlineable)
        {
            outlineable.HideInteractability();
            outlineable.enabled = false;
        }

        if (rb)
            rb.isKinematic = true;

        sniffable.RemoveCurrentTargetSniffable();

        activeDeliverable = false;
        Delivered = true;
        UpdatePersistentData();
    }

    public override DeliverableData GetPersistentData()
    {
        return gameManager.GameData.GetPersistentData(Data);
    }

    public override void SetFromPersistentData()
    {
        base.SetFromPersistentData();

        activeDeliverable = Data.activeDeliverable;
        Delivered = Data.delivered;
        thisTransform.position = Data.position;
        thisTransform.rotation = Data.rotation;
        thisTransform.localScale = Data.scale;

        if (Data.active)
            Enable(false); // Don't update the data
        else
            Disable(false);
    }

    public override void UpdatePersistentData()
    {
        base.UpdatePersistentData();

        Data.activeDeliverable = activeDeliverable;
        Data.delivered = Delivered;
        Data.position = thisTransform.position;
        Data.rotation = thisTransform.rotation;
        Data.scale = thisTransform.localScale;

        model = model ?? GetComponentInChildren<Model>();
        Data.active = activeObject = TransformUtils.IsModelActive(model);

        gameManager.GameData.AddPersistentData(Data);
    }

    private void Init()
    {
        biteable = biteable ?? GetComponent<Biteable>();
        pickupable = pickupable ?? GetComponent<Pickupable>();
        outlineable = outlineable ?? GetComponent<Outlineable>();
        sniffable = sniffable ?? GetComponent<Sniffable>();
        rb = rb ?? GetComponent<Rigidbody>();
        thisCollider = thisCollider ?? GetComponent<Collider>();
        thisTransform = thisTransform ?? GetComponent<Transform>();

        if (IsCrossSceneDeliverable)
        {
            biteable.OnBite.AddListener(OnCarried);
            biteable.OnRelease.AddListener(OnReleased);
        }

        thisCollider.enabled = false;
        biteable.enabled = false;
        outlineable.enabled = false;
    }

    private void OnCarried()
    {
        IsCarried = true;
    }

    private void OnReleased()
    {
        IsCarried = false;
        MoveToPersistent();
    }

    public void MoveToPersistent()
    {
        gameObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(persistentDeliverable.SceneName));
    }

    public void MoveToCurrentActive()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(SceneManager.GetActiveScene().name));
    }
}
