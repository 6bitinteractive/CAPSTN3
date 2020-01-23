using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual void Awake()
    {
        SingletonManager.Register<T>(this);
    }
}
