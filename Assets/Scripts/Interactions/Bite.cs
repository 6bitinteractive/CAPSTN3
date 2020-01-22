using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Bite : MonoBehaviour
{
    [SerializeField] private GameObject mouth;
    private bool isBiting;

    public GameObject Mouth { get => mouth; set => mouth = value; }
    public bool IsBiting { get => isBiting; set => isBiting = value; }

    public void BiteEvent(Interactor source, Biteable target)
    {
        target.Interact(source, target);
    }

    public void Release(Interactor source, Biteable target)
    {
        target.Release(source);
    }
}