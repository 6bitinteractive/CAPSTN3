using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class HungerEvent : UnityEvent<Hunger> { }

public class Hunger : MonoBehaviour
{
    [Header("Hunger Value")]
    [SerializeField] private float minHungerValue = 1f;
    [SerializeField] private float maxHungerValue = 100f;

    [Header("Hunger State Range")]
    [SerializeField] private float minFullHungerValue = 75f;
    [SerializeField] private float maxHungryHungerValue = 25f;

    public State CurrentState { get; private set; }
    public float CurrentValue { get; private set; }
    public float CurrentValueNormalized => CurrentValue / maxHungerValue;

    public HungerEvent OnEvaluateHunger;

    private static EventManager eventManager;
    private PlayerStats playerStats;

    private void OnEnable()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<PickupEvent, PickupData>(OnPickup);
    }

    private void OnDisable()
    {
        eventManager.Unsubscribe<PickupEvent, PickupData>(OnPickup);
    }

    private void Start()
    {
        playerStats = SingletonManager.GetInstance<PlayerStats>();

        if (playerStats.Hunger == 0)
            CurrentValue = playerStats.Hunger = maxHungerValue;
        else
            CurrentValue = playerStats.Hunger;
    }

    public void Eat(Food food)
    {
        CurrentValue += food.Value;
        EvaluateState(CurrentValue);
        Debug.LogFormat("{0} ate {1} [+{2}]", gameObject.name, food.DisplayName, food.Value);
        food.Consume(gameObject);
    }

    private void OnPickup(PickupData pickupData)
    {
        if (pickupData.type == PickupData.Type.Drop) { return; }

        Food food = pickupData.pickupable.GetComponent<Food>();
        if (food != null)
            Eat(food);
    }

    private State EvaluateState(float value)
    {
        CurrentValue = Mathf.Clamp(CurrentValue, minHungerValue, maxHungerValue);
        playerStats.Hunger = CurrentValue;

        if (value >= minFullHungerValue)
            CurrentState = State.Full;
        else if (value < minFullHungerValue && value > maxHungryHungerValue)
            CurrentState = State.Normal;
        else
            CurrentState = State.Hungry;

        OnEvaluateHunger.Invoke(this);

        return CurrentState;
    }

    public enum State
    {
        Full,
        Normal,
        Hungry
    }
}
