using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodBoyPointsHandler : MonoBehaviour
{
    [Header("GoodBoyPoints Value")]
    [SerializeField] private int minGoodBoyPoints = 1;
    [SerializeField] private int maxGoodBoyPoints = 100;

    [Header("GoodBoyPoints State Range")]
    [SerializeField] private int minFullGoodBoyPointsValue = 75;
    [SerializeField] private int maxFullGoodBoyPointsValue = 25;

    public State CurrentState { get; private set; }
    public int CurrentValue { get; private set; }
    public float CurrentValueNormalized => CurrentValue / maxGoodBoyPoints;

    public GoodBoyPointsHandlerEvent OnEvaluateGoodBoyPoints;

    private static EventManager eventManager;
    private PlayerStats playerStats;

    private void OnEnable()
    {
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<GoodBoyPointsEvent, GoodBoyPoints>(OnPointsUpdate);
    }

    private void OnDisable()
    {
        eventManager.Unsubscribe<GoodBoyPointsEvent, GoodBoyPoints>(OnPointsUpdate);
    }

    private void Start()
    {
        playerStats = SingletonManager.GetInstance<PlayerStats>();

        if (playerStats.GoodBoyPoints == 0)
            CurrentValue = playerStats.GoodBoyPoints = maxGoodBoyPoints;
        else
            CurrentValue = playerStats.GoodBoyPoints;

        EvaluateState(CurrentValue);
    }

    public void UpdatePoints(GoodBoyPoints goodBoyPoints)
    {
        CurrentValue += goodBoyPoints.Value;
        EvaluateState(CurrentValue);
    }

    private State EvaluateState(int value)
    {
        CurrentValue = Mathf.Clamp(CurrentValue, minGoodBoyPoints, maxGoodBoyPoints);

        playerStats = playerStats ?? SingletonManager.GetInstance<PlayerStats>();
        playerStats.GoodBoyPoints = CurrentValue;

        if (value >= minFullGoodBoyPointsValue)
            CurrentState = State.Good;
        else if (value < minFullGoodBoyPointsValue && value > maxFullGoodBoyPointsValue)
            CurrentState = State.Normal;
        else
            CurrentState = State.Bad;

        OnEvaluateGoodBoyPoints.Invoke(this);

        return CurrentState;
    }

    private void OnPointsUpdate(GoodBoyPoints goodBoyPoints)
    {
        if (goodBoyPoints != null)
            UpdatePoints(goodBoyPoints);
    }

    public enum State
    {
        Good,
        Normal,
        Bad
    }
}
