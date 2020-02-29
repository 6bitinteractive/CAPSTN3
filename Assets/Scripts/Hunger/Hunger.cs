using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public HungerDecay CurrentHungerDecay { get; set; }

    public HungerEvent OnEvaluateHunger;
    [HideInInspector] public HungerEvent OnInstantDecay;

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

        EvaluateState(CurrentValue);
    }

    private void Update()
    {
        Starve(CurrentHungerDecay);
    }

    public void Eat(Food food)
    {
        Satiate(food.Value);
        //Debug.LogFormat("{0} ate {1} [+{2}] - Hunger Level: {3}", gameObject.name, food.DisplayName, food.Value, CurrentValue);
        food.Consume(gameObject);
    }

    /// <summary>
    /// Increases hunger value. Use Eat() for increasing hunger by taking in food
    /// </summary>
    /// <param name="value">Increase hunger by this much.</param>
    public void Satiate(float value)
    {
        CurrentValue += value;
        EvaluateState(CurrentValue);
    }

    public void Starve(HungerDecay hungerDecay)
    {
        if (hungerDecay.duration > 0)
        {
            float decay = hungerDecay.decayValue * (Time.deltaTime / hungerDecay.duration);
            CurrentValue -= decay;
            //Debug.LogFormat("DECREASE: {0} | TOTAL: {1} in {2}", decay, hungerDecay.decayValue, hungerDecay.duration);
        }
        else if (hungerDecay.duration == 0)
        {
            CurrentValue -= hungerDecay.decayValue;
            //Debug.LogFormat("DECREASE INSTANTLY: {0}", hungerDecay.decayValue);
            OnInstantDecay.Invoke(this);
        }
        else
        {
            Debug.LogError("Hunger decay duration cannot be negative.");
        }

        //Debug.LogFormat("Starving: {0}/{1}", CurrentValue, maxHungerValue);
        EvaluateState(CurrentValue);
    }

    public void SwitchHungerDecay(HungerDecay hungerDecay)
    {
        CurrentHungerDecay = hungerDecay;
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

    private void OnPickup(PickupData pickupData)
    {
        if (pickupData.type == PickupData.Type.Drop) { return; }

        Food food = pickupData.pickupable.GetComponent<Food>();
        if (food != null)
            Eat(food);
    }

    public enum State
    {
        Full,
        Normal,
        Hungry
    }
}
