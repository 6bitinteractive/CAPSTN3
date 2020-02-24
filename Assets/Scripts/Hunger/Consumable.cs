using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] private float value = 20f;

    public string DisplayName => displayName;
    public float Value => value;

    protected virtual void OnDisable()
    { }

    public virtual void Consume(GameObject interactor)
    { }
}
