using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Bark : MonoBehaviour
{
    [SerializeField] private float radius = 5;
    [SerializeField] private AudioClip barkSfx;

    private static EventManager eventManager;
    private AudioSource audioSource;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void BarkEvent(Interactor source)
    {
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

                    InteractionData interactionData = new InteractionData();
                    interactionData.source = source;
                    interactionData.target = interactableTarget;
                    interactionData.interactionType = InteractionType.Bark;
                    eventManager.Trigger<InteractionEvent, InteractionData>(interactionData);
                }
            }
        }

        audioSource.clip = barkSfx;
        audioSource.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}