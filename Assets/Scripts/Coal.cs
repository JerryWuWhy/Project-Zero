// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
//
// public class Coal : MonoBehaviour
// {
//     public TextMeshProUGUI tmpTextC; 
//     public TextMeshProUGUI tmpTextI;
//     public TextMeshProUGUI tmpTextL;
//     public float interval = 1f; // Time in seconds between increments
//     public float increaseAmountCoal = 5f; // How much to increase per interval
//     public float increaseAmountIron = 7.5f;
//     public float increaseAmountLithium = 0.5f;
//     public float counterC = 0f; // The float value to increase
//     public float counterI = 0f;
//     public float counterL = 0f;
//     public string unit = "T"; // The unit to display after the number
//     private float timer = 0f;
//     public static Coal Inst { get; private set; }
//     public TimeManager timemanager;
//     private void Update()
//     {
//         
//         var countC = 0;
//         var countI = 0;
//         var countl = 0;
//         
//         var houses = DataManager.Inst.houses;
//         foreach (var houseData in houses)
//         {
//             if (houseData.outputType == OutputType.Coal)
//             {
//                 ++countC;
//             }
//             if (houseData.outputType == OutputType.Iron)
//             {
//                 ++countI;
//             }
//             if (houseData.outputType == OutputType.Lithium)
//             {
//                 ++countl;
//             }
//         }
//
//         increaseAmountCoal = 5f * (countC - MechanicalParts.Inst.counter);
//         increaseAmountIron = 7.5f * countI;
//         increaseAmountLithium = 0.5f * countl;
//
//         // Increment the timer based on time passed since last frame
//         if (timemanager.enableRealTimeUpdate)
//         {
//             timer += Time.deltaTime;
//         }
//         
//
//         // Check if enough time has passed based on the interval
//         if (timer >= interval)
//         {
//             // Reset the timer
//             timer = 0f;
//
//             // Increment the counter by the desired amount
//             counterC += increaseAmountCoal;
//             counterI += increaseAmountIron;
//             counterL += increaseAmountLithium;
//             
//                 tmpTextC.text = counterC.ToString("F2") + unit;
//                 tmpTextI.text = counterI.ToString("F2") + unit;
//                 tmpTextL.text = counterL.ToString("F2") + unit;
//         }
//     }
// }