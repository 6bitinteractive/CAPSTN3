using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scentSpeed = 0.9f;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject grayScalePostProcessingVolume;
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
        grayScalePostProcessingVolume.SetActive(true);

        Sniffable sniffable = CurrentDestination.GetComponent<Sniffable>();
        if (sniffable != null)
            eventManager.Trigger<ScentModeEvent, Sniffable>(sniffable);
    }

    public void DeactivateScentMode()
    {
        line.enabled = false;
        grayScalePostProcessingVolume.SetActive(false);
    }

    void Update()
    {
        if (CurrentDestination == null || !line.enabled) return;
        line.SetPosition(0, startPos.position);
        line.SetPosition(1, Vector3.MoveTowards(startPos.position, CurrentDestination.position, scentSpeed * Time.time));
    }
}