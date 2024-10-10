using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst { get; private set; }

    [Serializable]
    public class HouseData
    {
        public int houseId;
        public Vector3Int pos;
    }

    public List<HouseData> houses;

    private void Awake()
    {
        Inst = this;
    }
    
    public void SetHouseData(Vector3Int pos, int houseId)
    {
        foreach (var house in houses)
        {
            if (house.pos == pos)
            {
                house.houseId = houseId;
                return;
            }
        }
        houses.Add(new HouseData
        {
            houseId = houseId,
            pos = pos
        });
    }

    public HouseData GetHouseData(Vector3Int pos)
    {
        foreach (var house in houses)
        {
            if (house.pos == pos)
            {
                return house;
            }
        }

        return null;
    }
}