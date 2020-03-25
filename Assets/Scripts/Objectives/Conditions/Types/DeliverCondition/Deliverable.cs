using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sniffable))]

public class Deliverable : Persistable<DeliverableData>
{
    private Biteable biteable;
    private Pickupable pickupable;
    private Outlineable outlineable;
    private Sniffable sniffable;
    private Rigidbody rb;
    private Collider thisCollider;
    private Transform thisTransform;
    private bool activeDeliverable;

    public bool Delivered { get; private set; }

    private void Start()
    {
        Init();
        InitializeData();

        if (Delivered)
        {
            // Make it visible
            MakeVisible();
            MakeInteractable(false); // ...but not interactable
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

    public override void ResetData()
    {
        // Implement differently for Deliverables
        Data = new DeliverableData { guid = guidComponent.GetGuid() }; // Create a new default data
        UpdatePersistentData();
        SetFromPersistentData();
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

        thisCollider.enabled = false;
        biteable.enabled = false;
        outlineable.enabled = false;
    }
}
