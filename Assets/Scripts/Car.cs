using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public TextMeshProUGUI dicenumber;
    public bool motion = false;
    public float moveSpeed = 1f;
    private Vector3 lastMoveDirection;
    private Vector3Int initialPosition;
    private bool hasStartedMoving = false;
    public Button dice;
    private int stepsToMove = 0; // 用于记录随机生成的步数
    public GameObject prefab;
    public int currentroadid = 1;
    public Money money;
    public ConfigManager configmanager;
    
    private void Start()
    {
        motion = false;
        initialPosition = Vector3Int.RoundToInt(transform.position);
        dice.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        motion = true;

        // 生成随机数决定移动步数
        stepsToMove = UnityEngine.Random.Range(1, 7); // 随机数 [1, 6]
        dicenumber.text = stepsToMove.ToString();
        StartMovingToRoad();
        configmanager.newhousepanel.SetActive(false);
        configmanager.upgradebutton.gameObject.SetActive(false);
        configmanager.produce.SetActive(false);
    }

    public void StartMovingToRoad()
    {
        if (motion == true)
        {
            StartCoroutine(MoveToNearestRoad(stepsToMove));
        }
    }

    private void Update()
    {
        if (hasStartedMoving == false && transform.position.y > 0.1f)
        {
            hasStartedMoving = true;
            StartMovingToRoad();
        }
    }

    private System.Collections.IEnumerator MoveToNearestRoad(int steps)
    {
        while (steps > 0)
        {
            List<Vector3Int> roadPositions = new List<Vector3Int>();

            // 收集所有合法道路位置
            foreach (var road in ConfigManager.Inst.blockConfigs)
            {
                if (road.roadPos.x != initialPosition.x || road.roadPos.z != initialPosition.z)
                {
                    roadPositions.Add(road.roadPos);
                }
            }

            if (roadPositions.Count == 0)
            {
                yield break;
            }

            // 优先寻找上次方向上的道路
            Vector3Int? targetRoadPosition = FindRoadInLastDirection(roadPositions);

            // 如果上次方向没有合适的道路，寻找最近的道路
            if (targetRoadPosition == null)
            {
                targetRoadPosition = FindNearestRoad(roadPositions);
            }

            if (targetRoadPosition == null)
            {
                yield break;
            }

            // 移动到目标位置
            yield return MoveToPosition(targetRoadPosition.Value);

            // 更新方向向量
            lastMoveDirection = (targetRoadPosition.Value - transform.position).normalized;
            initialPosition = Vector3Int.RoundToInt(targetRoadPosition.Value);

            // 减少剩余步数
            steps--;
        }
        
        motion = false; // 停止运动
        dicenumber.text = "";

        // Debug OutputType counts
        DebugHouseCounts();

        Vector3 position = transform.position;
        Vector3Int positionInt = Vector3Int.RoundToInt(position);
        currentroadid += stepsToMove;
        if (currentroadid >= 45)
        {
            money.counter += 10;
        }
        if (currentroadid > 45)
        {
            currentroadid = 1;
        }

        configmanager.GetHouseOutputTypeByRoadId(currentroadid);
    }

    private void DebugHouseCounts()
    {
        // 输出 InstantMoney 类型房屋的数量
        int instantMoneyCount = ConfigManager.Inst.CountHousesWithOutputType(OutputType.InstantMoney);
        Debug.Log($"Number of houses with OutputType {OutputType.InstantMoney}: {instantMoneyCount}");
        if (currentroadid > 45)
        {
            money.counter += (10 * instantMoneyCount);
        }
        

        // 输出 DiceMoney 类型房屋的数量
        int diceMoneyCount = ConfigManager.Inst.CountHousesWithOutputType(OutputType.DiceMoney);
        Debug.Log($"Number of houses with OutputType {OutputType.DiceMoney}: {diceMoneyCount}");
        money.counter += (1 * diceMoneyCount);

        // 输出 Event 类型房屋的数量
        int eventCount = ConfigManager.Inst.CountHousesWithOutputType(OutputType.Event);
        Debug.Log($"Number of houses with OutputType {OutputType.Event}: {eventCount}");
        
        int gardenCount = ConfigManager.Inst.CountHousesWithOutputType(OutputType.Garden);
        Debug.Log($"Number of houses with OutputType {OutputType.Garden}: {gardenCount}");
    }

    // 优先在上次方向上寻找道路
    private Vector3Int? FindRoadInLastDirection(List<Vector3Int> roadPositions)
    {
        var nextPosition = Vector3Int.RoundToInt(transform.position + lastMoveDirection);
        if (roadPositions.Contains(nextPosition))
        {
            return nextPosition;
        }

        return null;
    }

    // 找到最近的道路
    private Vector3Int? FindNearestRoad(List<Vector3Int> roadPositions)
    {
        var curPosition = Vector3Int.RoundToInt(transform.position);
        var lastPosition = Vector3Int.RoundToInt(transform.position - lastMoveDirection);
        var positions = new[]
        {
            curPosition + new Vector3Int(1, 0, 0),
            curPosition + new Vector3Int(-1, 0, 0),
            curPosition + new Vector3Int(0, 0, 1),
            curPosition + new Vector3Int(0, 0, -1)
        };
        foreach (var pos in positions)
        {
            if (roadPositions.Contains(pos) && pos != lastPosition)
            {
                return pos;
            }
        }

        return null;
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            directionToTarget.y = 0; // 锁定 y 轴方向

            if (directionToTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}