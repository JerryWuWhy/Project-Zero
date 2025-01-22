using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbonSum : MonoBehaviour
{
    public Slider slider;
    public Slider sliders;
    public GameObject goodimage;
    public GameObject badimage;
    // Start is called before the first frame update
    public float number = 0f;
    public ConfigManager configmanager;
    public TimeManager timemanager;
    // public Gashpon gashpon;
    void Update()
    {
        number = 0f;
        var houses = DataManager.Inst.houses;
        foreach (var houseData in houses)
        {
            if (houseData.outputType == OutputType.DiceMoney || houseData.outputType == OutputType.InstantMoney ||houseData.outputType == OutputType.Event)
            {
                number++;
            }
            if (houseData.outputType == OutputType.Garden)
            {
                number--;
            }
            
        }

        if (timemanager.enableRealTimeUpdate)
        {
            slider.value -= (number*0.0005f) * Time.deltaTime;  
        }
        
        if (slider.value >= 1)
        {
            goodimage.SetActive(true);
            Time.timeScale = 0;
        }
        else if (slider.value <= 0)
        {
            badimage.SetActive(true);
            Time.timeScale = 0;
        }
    }
}

