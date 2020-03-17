using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueHandler))]

public class Talkable : MonoBehaviour, IInteractable
{
    [Tooltip("Should the object face the dog?")]
    [SerializeField] private bool faceSource = true;

    public UnityEvent OnTalk;

    private DialogueHandler dialogueHandler;
    private Animator animator;

    private void Start()
    {
        dialogueHandler = GetComponent<DialogueHandler>();
        animator = GetComponent<Animator>();
    }

    public void DisplayInteractability()
    {

    }

    public void Interact(Interactor source, IInteractable target)
    {
        //Debug.Log(source + "Is talking to " + target);
        if (!enabled) return;

        if (animator != null)
            animator.SetTrigger("Talk");

        OnTalk.Invoke();
        dialogueHandler.StartConversation();


       

        if (!source || !faceSource) { return; }
        StartCoroutine(RotateTowardsTarget(source.transform));
    }

    public void HideInteractability()
    {

    }

    IEnumerator RotateTowardsTarget (Transform targetTransform)
    {
        float duration = Quaternion.Angle(transform.rotation, targetTransform.rotation) / 100; // Get rotation duration
        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0; // Prevent y axis rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float currentTime = 0f;

        while (currentTime < duration)
        {
            yield return null;
            currentTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTime / duration);
        }

        transform.rotation = targetRotation;
    }
}