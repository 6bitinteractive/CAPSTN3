using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hunger))]

public class HungerDecayManager : MonoBehaviour
{
    [Header("Hunger Decay Values")]
    [SerializeField] private HungerDecay idle;
    [SerializeField] private HungerDecay biteCarry;
    [SerializeField] private HungerDecay bitePull;
    [SerializeField] private HungerDecay bark;
    [SerializeField] private HungerDecay dig;
    [SerializeField] private HungerDecay scentMode;

    private Hunger hunger;
    private static EventManager eventManager;

    private void Awake()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        hunger = GetComponent<Hunger>();
        hunger.SwitchHungerDecay(idle);

        // Make sure interactions such as bark are instantly applied
        bark.duration = dig.duration = 0;
    }

    private void OnEnable()
    {
        hunger.OnInstantDecay.AddListener(SwitchToIdle);
        eventManager.Subscribe<PickupEvent, PickupData>(OnBiteCarry);
        eventManager.Subscribe<PullEvent, PullData>(OnBitePull);
        eventManager.Subscribe<ScentModeEvent, ScentModeData>(OnScentMode);
        eventManager.Subscribe<InteractionEvent, InteractionData>(OnInteract);
    }

    private void OnDisable()
    {
        hunger.OnInstantDecay.RemoveListener(SwitchToIdle);
        eventManager.Unsubscribe<PickupEvent, PickupData>(OnBiteCarry);
        eventManager.Unsubscribe<PullEvent, PullData>(OnBitePull);
        eventManager.Unsubscribe<ScentModeEvent, ScentModeData>(OnScentMode);
        eventManager.Unsubscribe<InteractionEvent, InteractionData>(OnInteract);
    }

    private void OnBiteCarry(PickupData pickupData)
    {
        if (pickupData.type == PickupData.Type.Pickup)
            hunger.SwitchHungerDecay(biteCarry);
        else
            hunger.SwitchHungerDecay(idle);
    }

    private void OnBitePull(PullData pullData)
    {
        if (pullData.type == PullData.Type.Pull)
            hunger.SwitchHungerDecay(bitePull);
        else
            hunger.SwitchHungerDecay(idle);
    }

    private void OnScentMode(ScentModeData scentModeData)
    {
        if (scentModeData.state == ScentModeData.State.On)
            hunger.SwitchHungerDecay(scentMode);
        else
            hunger.SwitchHungerDecay(idle);
    }

    private void OnInteract(InteractionData interactionData)
    {
        switch (interactionData.interactionType)
        {
            case InteractionType.Bark:
                hunger.SwitchHungerDecay(bark);
                break;
            case InteractionType.Dig:
                hunger.SwitchHungerDecay(dig);
                break;
        }
    }

    private void SwitchToIdle(Hunger hunger)
    {
        hunger.SwitchHungerDecay(idle);
    }
}

[System.Serializable]
public class HungerDecay
{
    [Tooltip("How much should hunger decrease.")]
    public float decayValue;

    [Tooltip("How long should the decayValue be applied.\n\nA value of 0 means the decayValue is instantly applied.")]
    public float duration;
}
