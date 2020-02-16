using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public GameObject Target { get => target; set => target = value; }
}