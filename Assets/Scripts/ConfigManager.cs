using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum OutputType
{
    None,
    InstantMoney,
    DiceMoney,
    Event,
    Garden
}

public class ConfigManager : MonoBehaviour
{
    public PlacementManager pm;
    public Car car; // 引用 Car 类
    public static ConfigManager Inst { get; private set; }
    public GameObject newhousepanel;
    public Button id1;
    public Button id3;
    public Button id5;
    public Button id7;
    public Button cancel;
    public Money money;
    public GameObject log;
    public bool nobuilding = false;
    public Button upgradebutton;
    public Hud hud;
    public GameObject produce;
    [Serializable]
    public class HouseConfig
    {
        public int id;
        public int series;
        public int level;
        public GameObject prefab;
        public Sprite image;
        public int price;
        public OutputType outputType;
    }
    
    public int CountHousesWithOutputType(OutputType outputType)
    {
        // 获取 InitialHouseConfig 列表
        var currentConfigs = DataManager.Inst.houses;

        // 获取所有 HouseConfig 数据
        var allHouseConfigs = houseConfigs;

        // 遍历 InitialHouseConfig，查找匹配的 OutputType
        int count = 0;
        foreach (var currentConfig in currentConfigs)
        {
            // 根据 houseId 找到对应的 HouseConfig
            var houseConfig = allHouseConfigs.FirstOrDefault(h => h.id == currentConfig.houseId);
            if (houseConfig != null && houseConfig.outputType == outputType)
            {
                count++;
            }
        }

        return count;
    }

    [Serializable]
    public class BlockConfig
    {
        public int id;
        public Vector3Int roadPos;
        public Vector3Int housePos;
    }

    [Serializable]
    public class InitialHouseConfig
    {
        public int id;
        public int blockId;
        public int houseId;
    }

    public List<HouseConfig> houseConfigs;
    public List<BlockConfig> blockConfigs;
    public List<Transform> roads;
    public List<InitialHouseConfig> initialHouseConfigs;
    
    private void Awake()
    {
        Inst = this;

        // 添加按钮点击事件监听
        id1.onClick.AddListener(() => GenerateHouse(1)); // 生成 houseId 为 1 的房子
        id3.onClick.AddListener(() => GenerateHouse(3)); // 生成 houseId 为 2 的房子
        id5.onClick.AddListener(() => GenerateHouse(5));
        id5.onClick.AddListener(() => produce.SetActive(true));
        id7.onClick.AddListener(() => GenerateHouse(7));
        
        cancel.onClick.AddListener(() => newhousepanel.SetActive(false));
    }

    public List<HouseConfig> GetHouseConfigsByLevel(int level)
    {
        return houseConfigs.Where(h => h.level == level).ToList();
    }

    public HouseConfig GetHouseConfig(int id)
    {
        return houseConfigs.FirstOrDefault(h => h.id == id);
    }

    public HouseConfig GetHouseConfig(int series, int level)
    {
        return houseConfigs.FirstOrDefault(h => h.series == series && h.level == level);
    }

    public List<BlockConfig> GetRoadConfigs()
    {
        return blockConfigs.ToList();
    }

    public BlockConfig GetBlockConfig(int id)
    {
        return blockConfigs.FirstOrDefault(b => b.id == id);
    }

    [ContextMenu("Update Road Configs")]
    private void UpdateRoadConfigs()
    {
        blockConfigs.Clear();
        foreach (var road in roads)
        {
            var position = Vector3Int.FloorToInt(road.position);
            blockConfigs.Add(new BlockConfig
            {
                roadPos = position
            });
        }

        EditorUtility.SetDirty(this);
    }

    public OutputType GetHouseOutputTypeByRoadId(int blockId)
    {
        // 从 DataManager 的 houses 列表中获取对应的房屋数据
        var houseData = DataManager.Inst.houses.FirstOrDefault(h => h.blockId == blockId);

        if (houseData != null)
        {
            // 获取 BlockConfig 确定房子的位置
            var blockConfig = GetBlockConfig(blockId);
            if (blockConfig != null)
            {
                // 找到房子对应的 Transform
                var houseTransform = pm.GetHouseTransformByBlockId(blockConfig.id);
                if (houseTransform != null)
                {
                    ActivateAndConfigureButton(houseTransform, houseData);
                }
            }

            return houseData.outputType;
        }
        else
        {
            newhousepanel.gameObject.SetActive(true);
            RandomizeButtons();
        }

        return OutputType.None;
    }
    
    private void RandomizeButtons()
    {
        // 所有按钮存入一个列表
        var buttons = new List<Button> { id1, id3, id5, id7};

        // 随机打乱列表顺序
        buttons = buttons.OrderBy(_ => UnityEngine.Random.value).ToList();

        // 激活前两个按钮
        buttons[0].gameObject.SetActive(true);
        buttons[1].gameObject.SetActive(true);

        // 隐藏其他按钮
        for (int i = 2; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }
    private void ActivateAndConfigureButton(Transform houseTransform, DataManager.HouseData houseData)
    {
        // 确保按钮激活
        upgradebutton.gameObject.SetActive(true);

        // 每帧更新按钮位置
        StartCoroutine(UpdateButtonPosition(houseTransform));

        // 清除旧的点击事件，避免重复绑定
        upgradebutton.onClick.RemoveAllListeners();

        // 添加新的点击事件
        upgradebutton.onClick.AddListener(() =>
        {
            hud.OnUpgradeClick(houseData);
        });
    }

    private IEnumerator UpdateButtonPosition(Transform houseTransform)
    {
        while (upgradebutton.gameObject.activeSelf)
        {
            // 将房子的世界坐标转换为屏幕坐标
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(houseTransform.position);

            // 更新按钮的屏幕位置
            RectTransform buttonRect = upgradebutton.GetComponent<RectTransform>();
            buttonRect.position = screenPosition;

            yield return null; // 等待下一帧继续更新
        }
    }

    private void GenerateHouse(int houseId)
    {
        if (nobuilding) return;

        // 获取当前位置的 BlockConfig
        var blockConfig = GetBlockConfig(car.currentroadid);
        if (blockConfig == null)
        {
            Debug.LogWarning("BlockConfig not found for road ID: " + car.currentroadid);
            return;
        }

        // 检查目标位置是否已有房屋
        var existingHouse = DataManager.Inst.houses.FirstOrDefault(h => h.blockId == blockConfig.id);
        if (existingHouse != null)
        {
            nobuilding = true;
            return;
        }

        // 获取房屋配置
        var houseConfig = GetHouseConfig(houseId);
        if (houseConfig == null)
        {
            Debug.LogWarning("HouseConfig not found for house ID: " + houseId);
            return;
        }

        // 检查玩家是否有足够的货币
        if (money.counter < houseConfig.price)
        {
            log.SetActive(true); // 显示提示消息
            return;
        }

        // 扣除货币并建造房屋
        money.counter -= houseConfig.price;
        PlacementManager.Inst.PlaceHouse(blockConfig.id, houseConfig.id);

        // 更新 DataManager 的 houses 数据
        DataManager.Inst.SetHouseData(blockConfig.id, houseId);

        // 关闭新房屋面板
        newhousepanel.SetActive(false);
    }
}