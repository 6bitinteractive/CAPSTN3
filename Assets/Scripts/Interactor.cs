using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interactor : MonoBehaviour
{
    private IInteractable[] interactableTargets;
    private GameObject currentTarget;

    public IInteractable[] InteractableTargets { get => interactableTargets; set => interactableTargets = value; }
    public GameObject CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private void OnTriggerExit(Collider collider)
    {
        InteractableTargets = collider.gameObject.GetComponents<IInteractable>();
        InteractableTargets = null;
        CurrentTarget = null;
        //Debug.Log("No longer interacting with " + collider.name);
    }

    private void OnTriggerStay(Collider collider)
    {
        InteractableTargets = collider.gameObject.GetComponents<IInteractable>();

        if (InteractableTargets.Length == 0) return;
        foreach (var targets in InteractableTargets) targets.DisplayInteractability();
        currentTarget = collider.gameObject;
        //Debug.Log(currentTarget);
    }
}