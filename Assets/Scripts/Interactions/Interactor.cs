using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interactor : MonoBehaviour
{
    private List<GameObject> interactableTargetsGameObjectList = new List<GameObject>();
    private List<IInteractable> InteractableTargetsList = new List<IInteractable>();
    private GameObject currentTarget;
    private bool canInteract = true;
    public GameObject CurrentTarget { get => currentTarget; set => currentTarget = value; }
    public bool CanInteract { get => canInteract; set => canInteract = value; }

    private void OnTriggerExit(Collider collider)
    {
        if (canInteract)
        {
            IInteractable newTarget = collider.gameObject.GetComponent<IInteractable>();

            if (newTarget == null) return;
            {            
                newTarget.HideInteractability();
                ClearLists(newTarget.gameObject);
                CurrentTarget = null;
            }
        }
      //  Debug.Log("No longer interacting with " + collider.name);
    }


    // Reminder for myself if its too laggy perhaps changing it to onTriggerEnter would make more sense
    private void OnTriggerStay(Collider collider)
    {
        if (canInteract)
        {
            //Check if the new target is interactable
            IInteractable newTarget = collider.gameObject.GetComponent<IInteractable>();
         
            // Check if the new target exits
            if (newTarget == null) return;
            {
                CheckForDuplicatesInList(newTarget.gameObject);
                FindNearestGameObject();
                CreateNewInteractableList();
            }
        }
      //  Debug.Log("Current: " + currentTarget);
    }

    public GameObject FindNearestGameObject()
    {
        float distanceToNearestTarget = Mathf.Infinity;
        CurrentTarget = null;

        // Find the nearest object
        foreach (GameObject nearestTargets in interactableTargetsGameObjectList)
        {
            if (nearestTargets.activeInHierarchy)
            {
                float distanceToTarget = (nearestTargets.transform.position - gameObject.transform.position).sqrMagnitude;

                if (distanceToTarget < distanceToNearestTarget)
                {
                    distanceToNearestTarget = distanceToTarget;
                    CurrentTarget = nearestTargets; // Set current target to neareset target

                    //  Debug.Log("Nearest" + currentTarget + " " + currentTarget.transform.position);
                    //  Debug.DrawLine(gameObject.transform.position, currentTarget.transform.position);
                }
            }
 
        }
      // Debug.Log("Nearest" + CurrentTarget);
       return CurrentTarget;
    }

    private void CreateNewInteractableList()
    {
        // Clear the list of interactableTargetList
        foreach (IInteractable interactable in InteractableTargetsList)
            interactable.HideInteractability();

        InteractableTargetsList.Clear();

        // Create a new list based off the IInteractable componets in currentTarget
        foreach (IInteractable targets in CurrentTarget.GetComponents<IInteractable>())
        {
            CheckForDuplicatesInDisplayList(targets);
    
           // Debug.Log(targets);
           // Debug.Log(InteractableTargetsList.Count);
        }
    }

    private void CheckForDuplicatesInDisplayList(IInteractable newTarget)
    {
        if (newTarget.enabled)
        {
            if (InteractableTargetsList.Contains(newTarget))
            {
                // Debug.Log("Already contain " + newTarget);
                return;
            }

            else
            {
                InteractableTargetsList.Add(newTarget);
                DisplayInteractability(newTarget);
                // Debug.Log("Adding " + newTarget);
            }
        } 
    }

    private void DisplayInteractability(IInteractable newTarget)
    {
        newTarget.DisplayInteractability();
        HandleOutline(newTarget);
    }

    private void HandleOutline(IInteractable newTarget)
    {
        Outlineable outlineableTarget = newTarget.gameObject.GetComponent<Outlineable>();

        if (outlineableTarget != null)
        {
            // Turn on outline shader if there are other IInteractables enabled not including Outlineable
            if (newTarget.enabled && newTarget != outlineableTarget)
                outlineableTarget.enabled = true;

            // Else turn off Outlineable component
            else outlineableTarget.enabled = false;
        }
    }

    private void CheckForDuplicatesInList(GameObject newTarget)
    {
        if (interactableTargetsGameObjectList.Contains(newTarget.gameObject))
        {
           // Debug.Log("Already contain " + newTarget.gameObject);
            return;
        }

        else
        {
            interactableTargetsGameObjectList.Add(newTarget.gameObject);
          //  Debug.Log("Adding " + newTarget.gameObject.name);
        }
    }

    private void ClearLists(GameObject newTarget)
    {      
       foreach (IInteractable interactable in InteractableTargetsList) 
             interactable.HideInteractability();

        InteractableTargetsList.Clear();
        interactableTargetsGameObjectList.Remove(newTarget.gameObject);
    }
}