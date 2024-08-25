using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private void OnEnable()
    {
        TimeManager.OnDayChanged += UpdateTime;
        TimeManager.OnMonthChanged += UpdateTime;
        TimeManager.OnYearChanged += UpdateTime;
    }

    private void OnDisable()
    {
        TimeManager.OnDayChanged -= UpdateTime;
        TimeManager.OnMonthChanged -= UpdateTime;
        TimeManager.OnYearChanged -= UpdateTime;
    }  

    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Year:0000}.{TimeManager.Month:00}.{TimeManager.Day:00}";
    }
}