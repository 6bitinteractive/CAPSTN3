using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    [SerializeField] private float scentSpeed = 0.9f;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform startPos;
    private Transform currentDestination;

    public Transform CurrentDestination { get => currentDestination; set => currentDestination = value; }

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }

    public void ActivateScentMode()
    {
        Debug.Log("Enable Scent Mode");
        line.enabled = true;
    }

    public void DeactivateScentMode()
    {
        Debug.Log("Decativated");
        line.enabled = false;
    }

    void Update()
    {
        if (CurrentDestination == null || !line.enabled) return;
        line.SetPosition(0, startPos.position);
        line.SetPosition(1, Vector3.MoveTowards(startPos.position, CurrentDestination.position, scentSpeed * Time.time));     
    }
}