using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;  // Required if using Unity's built-in UI Text component
using TMPro;           // Required if using TextMeshPro

public class Gashpon : MonoBehaviour
{
    public PlacementManager placementmanager;
    public int times = 1;
    public RectTransform uiRoot; 
    public TextMeshProUGUI resultText; // 显示抽奖结果的 UI 文本
    public Button playButton; // 抽奖按钮
    public Camera mainCamera;
    public float spawnDistance = 10f; // 生成距离
    public float fixedY = 0.0f; // 固定高度
    public int carcountE = 0;
    public int carcountF = 0;
    private string[] prizes = { "Normal", "Rare", "Epic", "Legendary", "Unique" };
    private float[] prizeWeights = { 60, 25, 10, 4, 1 }; // 奖品权重
    private HashSet<Vector3> usedSpawnPositions = new HashSet<Vector3>(); // 已用生成点

    private void Start()
    {
        if (resultText != null)
        {
            resultText.text = "Press to try your luck!";
        }
    }

    public void Next()
    {
        if (resultText != null)
        {
            int index = GetWeightedPrizeIndex();
            resultText.text = $"{prizes[index]} Car Air Dropped!";
            for (int i = 0; i < times; i++)
            {
                SpawnObject(index);
            }
        }
    }

    public int GetWeightedPrizeIndex()
    {
        float totalWeight = prizeWeights.Sum();
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < prizeWeights.Length; i++)
        {
            cumulativeWeight += prizeWeights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return 0;
    }

    public void SpawnObject(int index)
    {
        Vector3 fixedSpawnPosition = new Vector3(7f, 5f, 14f); // 固定生成点坐标

        var carConfig = CarConfigManager.Inst.CarConfigs[index];
        GameObject objectToSpawn = carConfig.prefab;

        GameObject spawnedObject = Instantiate(objectToSpawn, fixedSpawnPosition, Quaternion.identity);

        // 启动移动逻辑
        StartMovingToClosestRoad(spawnedObject);

        usedSpawnPositions.Add(new Vector3(fixedSpawnPosition.x, 0f, fixedSpawnPosition.z));
        DataManager.Inst.AddCarData(fixedSpawnPosition, carConfig.id, carConfig.carbonType);
    }

    private void StartMovingToClosestRoad(GameObject car)
    {
        Transform targetRoad = FindClosestRoad(car.transform);

        if (targetRoad != null)
        {
            StartCoroutine(MoveToRoadCoroutine(car, targetRoad));
        }
        else
        {
            Debug.LogWarning("未找到最近的 Road。");
        }
    }

    private Transform FindClosestRoad(Transform carTransform)
    {
        var allRoads = GameObject.FindGameObjectsWithTag("Road")
                                 .Select(obj => obj.transform)
                                 .ToList();

        if (allRoads.Count == 0)
        {
            Debug.LogWarning("没有找到名为 'Road' 的物体。");
            return null;
        }

        return allRoads.OrderBy(road => Vector3.Distance(carTransform.position, road.position))
                       .FirstOrDefault();
    }

    private IEnumerator MoveToRoadCoroutine(GameObject car, Transform targetRoad)
    {
        float speed = 5f; // 移动速度

        while (car != null && Vector3.Distance(car.transform.position, targetRoad.position) > 0.1f)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, targetRoad.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void Update()
    {
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
}