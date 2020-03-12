using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI CountdownText;
    private Timer timer;

    void Start()
    {
        timer = GetComponent<Timer>();
        CountdownText.text = timer.CurrentCountdownValue.ToString("F2");
    }

    void Update()
    {
        if (timer.CountdownValue >= 0f)
            CountdownText.text = timer.CurrentCountdownValue.ToString("F2");

    }
}