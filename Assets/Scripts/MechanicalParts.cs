using System;
using UnityEngine;
using UnityEngine.UI; // Required if using Unity's built-in UI Text component
using TMPro; // Required if using TextMeshPro

public class MechanicalParts : MonoBehaviour
{
    public static MechanicalParts Inst { get; private set; }
    
    public TextMeshProUGUI tmpText; // TextMeshPro component if you use TextMeshPro
    public float interval = 1f; // Time in seconds between increments
    public float increaseAmount = 0.1f; // How much to increase per interval
    public float counter = 0f; // number of building
    public string unit = " T"; // The unit to display after the number
    private float timer = 0f;
    public Coal coal;
    public float countM = 0f;//stock
    public ConfigManager configmanager;

    private void Awake()
    {
        Inst = this;
    }

    private void Update()
    {
        counter = 0;
        var houses = DataManager.Inst.houses;
        foreach (var houseData in houses)
        {
            if (houseData.outputType == OutputType.Part && coal.counterC >= 5)
            {
                counter++;
            }
        }

        increaseAmount = 0.1f * counter;

        timer += Time.deltaTime;

        // Check if enough time has passed based on the interval
        if (timer >= interval)
        {
            // Reset the timer
            timer = 0f;
            countM += increaseAmount;


            tmpText.text = countM.ToString("F2") + unit;
        }
    }
}