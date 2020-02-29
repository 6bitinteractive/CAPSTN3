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
    private InteractionData interactionData;
    private static EventManager eventManager;

    public int DigPower { get => digPower; set => digPower = value; }
    public GameObject DigOffset { get => digOffset; set => digOffset = value; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void DigTerrainEvent(Interactor source, DigableTerrain target)
    {
        target.Interact(source, target);
        interactionData = new InteractionData()
        {
            source = source,
            target = target,
            interactionType = InteractionType.Dig
        };
        HandleDig();
    }

    public void DigEvent(Interactor source, Digable target)
    {
        target.Interact(source, target);
        interactionData = new InteractionData()
        {
            source = source,
            target = target,
            interactionType = InteractionType.Dig
        };
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

        eventManager.Trigger<InteractionEvent, InteractionData>(interactionData);
    }
}