using System;
using UnityEngine;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
using TMPro;           // Required if using TextMeshPro
using System.Collections.Generic;

public class Gashpon : MonoBehaviour
{
    public int times = 1;
    public HousePanel housepanel;
    private GameObject _clickedObject;
    private Vector3Int _clickedPos; 
    public RectTransform uiRoot; // If not used, you can remove this line
    public GameObject panel;
    public TextMeshProUGUI resultText;      // Make sure this is assigned in the Inspector
    public Button playButton;    // Ensure the button is assigned in the Inspector
    public Camera mainCamera;         // 主相机
    public float spawnDistance = 10f; // 生成的距离
    public float fixedY = 0.0f;
    public int carcountE = 0;
    public int carcountF = 0;
    private string[] prizes = { "Normal", "Rare", "Epic", "Legendary", "Unique" };
    // 对应奖品的权重数组，按顺序对应上面的 prizes
    private float[] prizeWeights = { 60, 25, 10, 4, 1 };  // 权重：Normal 最常见, Unique 最稀有
    
    public Hud hud;
    public TextMeshProUGUI LithiumAmount;
    public int LithiumCount = 0;
    public TextMeshProUGUI IronAmount;
    public int IronCount = 0;
    public Coal coal;

    private void Start()
    {
        // Ensure the text field is not null before assigning text
        if (resultText != null)
        {
            resultText.text = "Press to try your luck!";
        }
    }

    public void Next()
    {
        if (resultText != null)
        {
            housepanel.log.gameObject.SetActive(false);
            if (LithiumCount <= coal.counterL && IronCount <= coal.counterI)
            {
                coal.counterI -= IronCount;
                coal.counterL -= LithiumCount;
                prizeWeights[4] += LithiumCount * 40f;
                int index = GetWeightedPrizeIndex();
                resultText.text = $"{prizes[index]} Car Air Dropped!";
                for (int i = 0; i < times; i++)
                {
                    SpawnObject(index);
                }
                
                LithiumCount = 0;
                IronCount = 0;
                prizeWeights[4] = 1f;
                prizeWeights[0] = 60f;
            }
            else
            {
                housepanel.log.gameObject.SetActive(true);
            }
        }
    }

    public int GetWeightedPrizeIndex()
    {
        float totalWeight = 0;

        // 计算权重总和
        foreach (float weight in prizeWeights)
        {
            totalWeight += weight;
        }

        // 生成一个在 [0, totalWeight) 之间的随机数
        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        // 根据随机数来选择对应的奖品
        float cumulativeWeight = 0;
        for (int i = 0; i < prizeWeights.Length; i++)
        {
            cumulativeWeight += prizeWeights[i];
            if (randomValue < cumulativeWeight)
            {
                return i; // 返回选中的奖品
            }
        }

        return 0;  // 默认返回第一个奖品（理论上不会发生）
    }

    public void SpawnObject(int index)
    {
        // In the viewport, choose a random point for x and z
        float randomX = UnityEngine.Random.Range(0.15f, 0.9f);
        float randomY = UnityEngine.Random.Range(0.15f, 0.9f);
        Vector3 randomViewportPoint = new Vector3(randomX, randomY, spawnDistance);

        // Convert viewport coordinates to world coordinates
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(randomViewportPoint);

        // Set the y-axis to the fixed height
        worldPosition.y = fixedY;

        // Instantiate the object from the list based on the random index
        var carConfig = CarConfigManager.Inst.CarConfigs[index];
        GameObject objectToSpawn = carConfig.prefab;
        Instantiate(objectToSpawn, worldPosition, Quaternion.identity);
        DataManager.Inst.AddCarData(worldPosition, carConfig.id, carConfig.carbonType);
    }

    private void Update()
    {
        LithiumAmount.text = LithiumCount.ToString();
        IronAmount.text = IronCount.ToString();
        
        carcountE = 0;
        carcountF = 0;
        var cars = DataManager.Inst.cars;
        foreach (var carData in cars)
        {
            if (carData.carbonType == CarbonType.decrease)
            {
                ++carcountE;
            }
            else if (carData.carbonType == CarbonType.increase)
            {
                ++carcountF;
            }

        }
    }

    public void OnLithiumRoll()
    {
        LithiumCount += 1;
    }

    public void OnIronRoll()
    {
        IronCount += 1;
        times += 1;
    }
}