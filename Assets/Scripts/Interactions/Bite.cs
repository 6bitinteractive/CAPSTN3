using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Bite : MonoBehaviour
{
    [SerializeField] private GameObject mouth;
    [SerializeField] private AudioClip biteSfx;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;
    private bool isBiting;

    public GameObject Mouth { get => mouth; set => mouth = value; }
    public bool IsBiting { get => isBiting; set => isBiting = value; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BiteEvent(Interactor source, Biteable target)
    {
        target.Interact(source, target);
        audioSource.clip = biteSfx;
        audioSource.Play();

        if (animator == null) return;
        Animate();
    }

    public void Release(Interactor source, Biteable target)
    {
        target.Release(source);

        if (animator == null) return;
        Deanimate();
    }

    public void Animate()
    {
        animator.SetBool("IsBiting", true);
    }

    public void Deanimate()
    {
        animator.SetBool("IsBiting", false);
    }
}