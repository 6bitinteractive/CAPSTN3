using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scentSpeed = 0.9f;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject postProcessingEffect;
    private Transform currentDestination;
    private static EventManager eventManager;

    public Transform CurrentDestination { get => currentDestination; set => currentDestination = value; }

    void Start()
    {
        if (mainCamera == null) Debug.LogError("main camera is null please set main camera");
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void ActivateScentMode()
    {
        line.enabled = true;
        postProcessingEffect.SetActive(true);

        Sniffable sniffable = null;
        if (CurrentDestination != null)
        {
            sniffable = CurrentDestination.GetComponent<Sniffable>();
        }

        ScentModeData scentModeData = new ScentModeData()
        {
            sniffable = sniffable,
            state = ScentModeData.State.On
        };
        eventManager.Trigger<ScentModeEvent, ScentModeData>(scentModeData);
    }

    public void DeactivateScentMode()
    {
        line.enabled = false;
        postProcessingEffect.SetActive(false);

        ScentModeData scentModeData = new ScentModeData()
        {
            sniffable = null,
            state = ScentModeData.State.Off
        };
        eventManager.Trigger<ScentModeEvent, ScentModeData>(scentModeData);
    }

    void Update()
    {
        if (CurrentDestination == null || !line.enabled) return;
        line.SetPosition(0, startPos.position);
        line.SetPosition(1, Vector3.MoveTowards(startPos.position, CurrentDestination.position, scentSpeed * Time.time));
    }
}