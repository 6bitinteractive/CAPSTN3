using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject targetWithinRange;

    public GameObject Target { get => target; set => target = value; }
    public GameObject TargetWithinRange { get => targetWithinRange; set => targetWithinRange = value; }
}