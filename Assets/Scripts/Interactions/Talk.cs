using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Talk : MonoBehaviour
{
    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void TalkEvent(Interactor source, Talkable target)
    {
        target.Interact(source, target);

        InteractionData interactionData = new InteractionData
        {
            source = source,
            target = target.gameObject,
            interactionType = InteractionType.Talk
        };

        eventManager.Trigger<InteractionEvent, InteractionData>(interactionData);
    }
}