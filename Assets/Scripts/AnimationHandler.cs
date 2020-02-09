using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHandler : MonoBehaviour
{
    public UnityEvent OnAnimStart = new UnityEvent();
    public UnityEvent OnAnimEnded = new UnityEvent();

    public void StartAnimation()
    {
        OnAnimStart.Invoke();
    }

    public void EndAnimation()
    {
        OnAnimEnded.Invoke();
    }
}