using System;
using System.Collections.Generic;
using System.Reflection;
using Habby.Storage;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst { get; private set; }

    private StorageContainer _dataContainer;

    [Serializable]
    public class HouseData
    {
        public int houseId;
        public Vector3Int pos;
        public OutputType outputType;
    }

    public List<HouseData> houses;

    private void Awake()
    {
        Inst = this;
        _dataContainer = Storage.GetContainer("Data");
        houses = _dataContainer.Get("houses", new List<HouseData>());
        cars = _dataContainer.Get("cars", new List<CarData>());
        roads = _dataContainer.Get("roads", new List<RoadData>());
        
        var test = _dataContainer.Get("test", new Vector3Int(1, 1, 1));
        _dataContainer.Save();
        // Storage.ClearAll();
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

        var houseConfig = ConfigManager.Inst.GetHouseConfig(houseId);
        houses.Add(new HouseData
        {
            houseId = houseId,
            pos = pos,
            outputType = houseConfig.outputType
        });
        _dataContainer.Save();
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

    [Serializable]
    public class CarData
    {
        public int carId;
        public Vector3 pos;
        public CarbonType carbonType;
    }

    public List<CarData> cars;
    

    public void AddCarData(Vector3 pos, int carId, CarbonType carbonType)
    {
        cars.Add(new CarData
        {
            carId = carId,
            pos = pos,
            carbonType = carbonType
        });
        _dataContainer.Save();
    }


    public CarData GetCarData(Vector3Int pos)
    {
        foreach (var car in cars)
        {
            if (car.pos == pos)
            {
                return car;
            }
        }

        return null;
    }


    [Serializable]
    public class RoadData
    {
        public Vector3Int pos;
    }

    public List<RoadData> roads;

    public void SetRoadData(Vector3Int pos)
    {
        roads.Add(new RoadData
        {
            pos = pos,
        });
        _dataContainer.Save();
    }


    public RoadData GetRoadData(Vector3Int pos)
    {
        foreach (var road in roads)
        {
            if (road.pos == pos)
            {
                return road;
            }
        }

        return null;
    }
}