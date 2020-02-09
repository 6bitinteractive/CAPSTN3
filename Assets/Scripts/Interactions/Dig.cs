using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour
{
    [SerializeField] private int digPower = 1;
    [SerializeField] private AudioClip digSfx;
    [SerializeField] private GameObject digOffset;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;

    public int DigPower { get => digPower; set => digPower = value; }
    public GameObject DigOffset { get => digOffset; set => digOffset = value; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DigTerrainEvent(Interactor source, DigableTerrain target)
    {
        target.Interact(source, target);
        HandleDig();
    }

    public void DigEvent(Interactor source, Digable target)
    {
        target.Interact(source, target);
        HandleDig();
    }

    public void Animate()
    {
        animator.SetTrigger("Dig");
    }

    public void HandleDig()
    {
        audioSource.clip = digSfx;
        audioSource.Play();

        if (animator == null) return;
        {
            Animate();
        }      
    }
}