using System;
using UnityEngine;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
using TMPro;           // Required if using TextMeshPro
using System.Collections.Generic;

public class Gashpon : MonoBehaviour
{
    public PlacementManager placementmanager;
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

    // 用于记录已使用的生成点坐标
    private HashSet<Vector3> usedSpawnPositions = new HashSet<Vector3>();

    public void SpawnObject(int index)
    {
        // 查找所有 Transform 类型的对象，并筛选出指定名称的对象
        Transform[] allTransforms = FindObjectsOfType<Transform>();
        List<Transform> spawnPoints = new List<Transform>();

        foreach (var trans in allTransforms)
        {
            if (trans.name == "Road")
            {
                spawnPoints.Add(trans);
            }
        }

        // 检查是否找到了符合名称的对象
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("没有找到名为 'Road' 的生成点。");
            return;
        }

        // 过滤掉已使用的生成点，确保只选择未使用的生成点
        List<Transform> availableSpawnPoints = spawnPoints.FindAll(sp => !usedSpawnPositions.Contains(new Vector3(sp.position.x, 0f, sp.position.z)));

        // 检查是否还有未使用的生成点
        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("所有生成点都已被使用，无法生成新的对象。");
            return;
        }
    
        // 随机选择一个未使用的生成点
        Transform selectedSpawnPoint = availableSpawnPoints[UnityEngine.Random.Range(0, availableSpawnPoints.Count)];
        Vector3 spawnPosition = selectedSpawnPoint.position;
    
        // 设置生成物体的 y 坐标为 fixedY
        spawnPosition.y = fixedY; // 设置固定高度
    
        // 根据索引从 CarConfigManager 中获取对象的配置
        var carConfig = CarConfigManager.Inst.CarConfigs[index];
        GameObject objectToSpawn = carConfig.prefab;
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    
        // 启动 Car 的移动逻辑
        Car carComponent = spawnedObject.GetComponent<Car>();
        // if (carComponent != null)
        // {
        //     carComponent.StartMovingToRoad();
        //     Debug.LogWarning("1");
        // }
    
        // 将该位置（忽略高度）添加到已使用的生成点集合中
        usedSpawnPositions.Add(new Vector3(spawnPosition.x, 0f, spawnPosition.z));
    
        // 添加对象数据到数据管理器
        DataManager.Inst.AddCarData(spawnPosition, carConfig.id, carConfig.carbonType);
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