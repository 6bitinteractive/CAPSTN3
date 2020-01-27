using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour
{
    [SerializeField] private int digPower = 1;
    [SerializeField] private AudioClip digSfx;
    private AudioSource audioSource;

    public int DigPower { get => digPower; set => digPower = value; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DigTerrainEvent(Interactor source, DigableTerrain target)
    {
        target.Interact(source, target);
    }

    public void DigEvent(Interactor source, Digable target)
    {
        target.Interact(source, target);
        audioSource.clip = digSfx;
        audioSource.Play();
    }
}