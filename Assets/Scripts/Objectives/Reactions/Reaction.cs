using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reaction : MonoBehaviour
{
    public virtual void Execute()
    {
        Debug.LogFormat("{0} has been executed.", gameObject.name);
    }
}
