using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interactor : MonoBehaviour
{
    private IInteractable[] interactableTargets;
    private GameObject currentTarget;
    private bool canInteract = true;

    public IInteractable[] InteractableTargets { get => interactableTargets; set => interactableTargets = value; }
    public GameObject CurrentTarget { get => currentTarget; set => currentTarget = value; }
    public bool CanInteract { get => canInteract; set => canInteract = value; }

    private void OnTriggerExit(Collider collider)
    {
        if (canInteract)
        {
            InteractableTargets = collider.gameObject.GetComponents<IInteractable>();           
            if (interactableTargets != null)
            {
                if (InteractableTargets.Length == 0) return;
                foreach (var targets in InteractableTargets) targets.HideInteractability();
                CurrentTarget = null;
                InteractableTargets = null;
            }
        }    
        //Debug.Log("No longer interacting with " + collider.name);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (canInteract)
        {
            InteractableTargets = collider.gameObject.GetComponents<IInteractable>();
            if (interactableTargets != null)
            {
                if (InteractableTargets.Length == 0) return;
                foreach (var targets in InteractableTargets) targets.DisplayInteractability();
                currentTarget = collider.gameObject;
            }
        }
        //Debug.Log(currentTarget);
    }
}