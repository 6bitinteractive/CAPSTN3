using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactor))]
public class Talk : MonoBehaviour
{
    public void TalkEvent(Interactor source, Talkable target)
    {
        target.Interact(source, target);
    }
}