using UnityEngine;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
using TMPro;           // Required if using TextMeshPro

public class Money : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // TextMeshPro component if you use TextMeshPro
    public float interval = 1f;     // Time in seconds between increments
    public float increaseAmount = 0.1f; // How much to increase per interval
    public float counter = 0f;     // The float value to increase
    public string unit = " K"; // The unit to display after the number
    public Car car;
    public static Money Inst { get; private set; }
    private float timer = 0f;
    public TimeManager timemanager;
    
    void Update()
    {
                increaseAmount = 0.01f;
                // Increment the timer based on time passed since last frame
                if (timemanager.enableRealTimeUpdate)
                {
                    timer += Time.deltaTime;
                }
                

                // Check if enough time has passed based on the interval
                if (timer >= interval)
                {
                    // Reset the timer
                    timer = 0f;

                    // Increment the counter by the desired amount
                    counter += increaseAmount;

                    // Update the UI text with float value formatted to 2 decimal places and add the unit
                    if (tmpText != null)
                        tmpText.text = counter.ToString("F2") + unit;
                }
    }
}
