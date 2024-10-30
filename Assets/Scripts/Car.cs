using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Vector3 lastMoveDirection;
    private Vector3Int initialPosition;
    private bool hasStartedMoving = false;
    private void Start()
    {
        initialPosition = Vector3Int.RoundToInt(transform.position);
        
    }

    public void StartMovingToRoad()
    {

            StartCoroutine(MoveToNearestRoad());
        
    }

    private void Update()
    {
        if (hasStartedMoving == false && transform.position.y > 0.1f)
        {
            hasStartedMoving = true;
            StartMovingToRoad();
        }
    }

    private System.Collections.IEnumerator MoveToNearestRoad()
    {
        while (true)
        {
            List<Vector3Int> roadPositions = new List<Vector3Int>();

            foreach (var road in DataManager.Inst.roads)
            {
                if (road.pos.x != initialPosition.x || road.pos.z != initialPosition.z)
                {
                    roadPositions.Add(road.pos);
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

            // 移动到目标位置
            yield return MoveToPosition(targetRoadPosition.Value);

            // 更新方向向量
            lastMoveDirection = (targetRoadPosition.Value - transform.position).normalized;
            initialPosition = Vector3Int.RoundToInt(targetRoadPosition.Value);
        }
    }

    // 优先在上次方向上寻找道路
    private Vector3Int? FindRoadInLastDirection(List<Vector3Int> roadPositions)
    {
        Vector3 nextPosition = transform.position + lastMoveDirection;

        Vector3Int? closestInDirection = null;
        float minDistanceInDirection = float.MaxValue;

        foreach (var roadPos in roadPositions)
        {
            float distance = Vector3.Distance(nextPosition, roadPos);
            // 允许一定范围的偏差，找到接近上次方向的目标
            if (distance < minDistanceInDirection && distance < 1.5f)
            {
                minDistanceInDirection = distance;
                closestInDirection = roadPos;
            }
        }

        return closestInDirection;
    }

    // 找到最近的道路
    private Vector3Int FindNearestRoad(List<Vector3Int> roadPositions)
    {
        Vector3Int closestRoad = Vector3Int.zero;
        float minDistance = float.MaxValue;

        foreach (var roadPos in roadPositions)
        {
            float distance = Vector3.Distance(transform.position, roadPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoad = roadPos;
            }
        }

        return closestRoad;
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // 计算朝向目标的方向向量，只考虑 x 和 z 轴
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            directionToTarget.y = 0; // 锁定 y 轴方向

            // 设置物体的朝向，仅影响 x 和 z 轴
            if (directionToTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }

            // 向目标位置移动
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
            yield return null;
        }
    }
}