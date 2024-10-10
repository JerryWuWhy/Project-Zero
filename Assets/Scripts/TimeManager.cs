using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
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
    public bool enableRealTimeUpdate = false; // Unity UI Text (or)
    public TextMeshProUGUI tmpText;
    

// Start is called before the first frame update
    void Start()
    {
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

        else
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
        tmpText.text = "Pause";
        if (_paused = !_paused)
        {
            tmpText.text = "Continue";
            Pause(_paused);
            
        }
        else
        {
            Pause(_paused);
        }

            

 

        
            
        
        
        

    }

    private void Pause(bool isPaused)
    {
        enableRealTimeUpdate = !isPaused;
    }

}
