using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour
{
    private Sniff sniff;

    public void SetCurrentTarget()
    {
        sniff = FindObjectOfType<Sniff>();
        sniff.CurrentDestination = gameObject.transform;
    }

    public void RemoveCurrentTargetSniffable()
    {
        sniff.CurrentDestination = null;
    }
}