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
    public float carbonvalue = 0.005f;
    // Start is called before the first frame update
    public float coalnumber = 0f;
    public ConfigManager configmanager;
    public float partnumber = 0f;
    public TimeManager timemanager;
    public Gashpon gashpon;
    // Update is called once per frame
    void Update()
    {
        coalnumber = 0f;
        partnumber = 0f;
        var houses = DataManager.Inst.houses;
        foreach (var houseData in houses)
        {
            if (houseData.outputType == OutputType.Iron || houseData.outputType == OutputType.Lithium ||houseData.outputType == OutputType.Coal)
            {
                coalnumber++;
            }
            else if (houseData.outputType == OutputType.Part)
            {
                partnumber++;
            }
            
        }

        if (timemanager.enableRealTimeUpdate)
        {
            slider.value -= (carbonvalue + coalnumber*0.001f + partnumber*0.001f - gashpon.carcountE*0.01f + gashpon.carcountF*0.005f) * Time.deltaTime;  
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
        if (timemanager.enableRealTimeUpdate)
        {
            sliders.value -= (carbonvalue + coalnumber*0.001f + partnumber*0.001f - gashpon.carcountE*0.01f + gashpon.carcountF*0.005f) * Time.deltaTime;  
        }
        
        if (sliders.value >= 1)
        {
            goodimage.SetActive(true);
            Time.timeScale = 0;
        }
        else if (sliders.value <= 0)
        {
            badimage.SetActive(true);
            Time.timeScale = 0;
        }
    }
}

