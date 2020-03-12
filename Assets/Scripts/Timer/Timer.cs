using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float CountdownValue = 5;
    public UnityEvent OnCountdownStart;
    public UnityEvent OnCountdownEnd;

    private float currentCountdownValue;

    public float CurrentCountdownValue
    {
        get
        {
            return currentCountdownValue;
        }

        set
        {
            currentCountdownValue = value;
        }
    }

    void Start()
    {
        if (OnCountdownEnd == null) OnCountdownEnd = new UnityEvent();
        if (OnCountdownStart == null) OnCountdownStart = new UnityEvent();

        if (OnCountdownStart != null) OnCountdownStart.Invoke();
        CurrentCountdownValue = CountdownValue;
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        // If countdown ended call CountdownEnd function
        if (CurrentCountdownValue <= 0)
        {
            CurrentCountdownValue = 0f;
            CountdownEnd();
        }
    }

    public IEnumerator StartCountdown()
    {
        while (CurrentCountdownValue > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime); // Scale timer
            CurrentCountdownValue -= Time.deltaTime; // Reduce timer
        }
    }

    private void CountdownEnd()
    {
        if (OnCountdownEnd != null) OnCountdownEnd.Invoke(); // Broadcast to listeners that countdown has ended
    }

    public void ResetTimer()
    {
        CurrentCountdownValue = CountdownValue;
        StartCoroutine(StartCountdown());
    }
}
