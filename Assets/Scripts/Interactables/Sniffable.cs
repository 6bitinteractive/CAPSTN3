using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour
{
    private Sniff sniff;

    public void SetCurrentTarget()
    {
        sniff = FindObjectOfType<Sniff>();
        if (sniff == null || !sniff.enabled) return;

        sniff.CurrentDestination = gameObject.transform;
    }

    public void RemoveCurrentTargetSniffable()
    {
        if (sniff != null)
            sniff.CurrentDestination = null;
    }
}