﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Bark : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    [SerializeField] private AudioClip barkSfx;
    [SerializeField] private Animator animator;

    private static EventManager eventManager;
    private AudioSource audioSource;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void BarkEvent(Interactor source)
    {
        InteractionData interactionData = new InteractionData
        {
            source = source,
            target = null,
            interactionType = InteractionType.Bark
        };

        // Set targets to anything that overlaps with sphere
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, -1, QueryTriggerInteraction.Ignore); //Ignore trigger is to prevent being called multiple times on one object
        {
            // Apply BarkEvent to all targets
            foreach (var target in targets)
            {
                Barkable barkable = target.GetComponent<Barkable>();
                IInteractable interactableTarget = target.GetComponent<IInteractable>();
                if (barkable != null)
                {
                    barkable.Interact(source, interactableTarget);
                    //Debug.Log("Barking at " + target.gameObject.name);

                    interactionData.target = target.gameObject;
                }
            }

        }

        // FIX: the target set in interactionData only contains the last object set in the foreach loop
        eventManager.Trigger<InteractionEvent, InteractionData>(interactionData);

        audioSource.clip = barkSfx;
        audioSource.Play();

        if (animator == null) return;
        Animate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Animate()
    {
        animator.SetTrigger("Bark");
    }
}