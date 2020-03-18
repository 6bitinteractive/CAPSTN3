using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBoyPoints : MonoBehaviour
{
    [Tooltip("Can be negative.")]
    [SerializeField] private int value = 10;

    public int Value => value;

    private static EventManager eventManager;

    private void Start()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    public void UpdateGoodBoyPoints()
    {
        eventManager.Trigger<GoodBoyPointsEvent, GoodBoyPoints>(this);
    }
}
