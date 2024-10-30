using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI; // Required if using Unity's built-in UI Text component
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static Action OnDayChanged;
    public static Action OnMonthChanged;
    public static Action OnYearChanged;
    public static int Day { get; private set; }
    public static int Month { get; private set; }
    public static int Year { get; private set; }
    public float dayToRealTime = 1f;
    private float timer;
    public bool enableRealTimeUpdate = true;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI btnText;

    private void OnEnable()
    {
        OnDayChanged += UpdateTime;
        OnMonthChanged += UpdateTime;
        OnYearChanged += UpdateTime;
    }

    private void OnDisable()
    {
        OnDayChanged -= UpdateTime;
        OnMonthChanged -= UpdateTime;
        OnYearChanged -= UpdateTime;
    }

    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Year:0000}.{TimeManager.Month:00}.{TimeManager.Day:00}";
      
    }

// Start is called before the first frame update
    void Start()
    {
        Pause(false);
        Day = 01;
        Month = 01;
        Year = 2025;
        timer = dayToRealTime;
    }

    // Update is called once per frame
    void Update()
    {
        var passDay = Time.deltaTime / dayToRealTime;
        timer -= Time.deltaTime;
        if (!enableRealTimeUpdate)
        {
            return;
        }

        {
            if (timer <= 0)
            {
                Day++;
                OnDayChanged?.Invoke();
                if (Day >= 30)
                {
                    Month++;
                    Day = 1;
                    OnMonthChanged?.Invoke();
                }

                if (Month >= 12)
                {
                    Year++;
                    Month = 1;
                    OnYearChanged?.Invoke();
                }

                timer = dayToRealTime;
            }
        }
    }

    private bool _paused;

    public void OnClickPause()
    {
        btnText.text = "Pause";
        if (_paused = !_paused)
        {
            btnText.text = "Continue";
            Pause(_paused);
        }
        else
        {
            Pause(_paused);
        }
    }

    public void Pause(bool isPaused)
    {
        enableRealTimeUpdate = !isPaused;
    }
}