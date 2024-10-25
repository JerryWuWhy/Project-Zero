using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum CarbonType
{
    decrease,
    increase
}
public class CarConfigManager : MonoBehaviour
{
    public static CarConfigManager Inst { get; private set; }

    [Serializable]
    public class CarConfig
    {
        public int id;
        public int carboneffect;
        [FormerlySerializedAs("carbontype")] public CarbonType carbonType;
        public GameObject prefab;
        public int price;
    }

    public CarConfig[] CarConfigs;

    private void Awake()
    {
        Inst = this;
    }

    public CarConfig GetCarConfig(int id)
    {
        foreach (var CarConfig in CarConfigs)
        {
            if (CarConfig.id == id)
            {
                return CarConfig;
            }
        }

        return null;
    }
}