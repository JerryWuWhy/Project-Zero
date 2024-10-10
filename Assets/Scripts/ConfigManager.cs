using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Inst { get; private set; }
    
    [Serializable]
    public class HouseConfig
    {
        public int id;
        public int series;
        public int level;
        public GameObject prefab;
        public Sprite image;
    }

    public HouseConfig[] houseConfigs;

    private void Awake()
    {
        Inst = this;
    }

    public List<HouseConfig> GetHouseConfigsByLevel(int level)
    {
        var configs = new List<HouseConfig>();
        foreach (var houseConfig in houseConfigs)
        {
            if (houseConfig.level == level)
            {
                configs.Add(houseConfig);
            }
        }

        return configs;
    }

    public HouseConfig GetHouseConfig(int id)
    {
        foreach (var houseConfig in houseConfigs)
        {
            if (houseConfig.id == id)
            {
                return houseConfig;
            }
        }

        return null;
    }

    public HouseConfig GetHouseConfig(int series, int level)
    {
        foreach (var houseConfig in houseConfigs)
        {
            if (houseConfig.series == series && houseConfig.level == level)
            {
                return houseConfig;
            }
        }

        return null;
    }
}