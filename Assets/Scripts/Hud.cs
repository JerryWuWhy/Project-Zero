using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hud : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    public HousePanel housePanel;
    public HouseButton houseButton;
    public RoadButton roadButton;
    public DeleteButton deleteButton;
    public TextMeshProUGUI price;
    public GameObject upgradeButton;
    public Money money;
    public HousePanel housepanel;
    public GameObject coalpanel;
    public GameObject Gashponpanel;
    public GameObject Partpanel;
    public enum State
    {
        None,
        RoadPlacement,
        HousePlacement,
        Remove,
    }

    public static Hud Inst { get; private set; }
    public DataManager.HouseData ClickHouseData { get; private set; }

    private State _state;

    private void Awake()
    {
        Inst = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_state == State.RoadPlacement)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                RoadManager.Inst.PlaceRoad(pos.Value);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_state == State.RoadPlacement)
        {
            RoadManager.Inst.FinishPlacingRoad();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        housepanel.log.gameObject.SetActive(false);
        if (_state == State.None)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                var houseData = DataManager.Inst.GetHouseData(pos.Value);
                ClickHouseData = houseData;
                if (houseData != null)
                {
                    var houseConfig = ConfigManager.Inst.GetHouseConfig(houseData.houseId);
                    if (houseConfig.series is 6)
                    {
                        coalpanel.SetActive(true);
                    }
                    if (houseConfig.series is 7)
                    {
                        Gashponpanel.SetActive(true);
                    }
                    if (houseConfig.series is 5)
                    {
                        Partpanel.SetActive(true);
                    }
                    else
                    {
                        var upgradedConfig =
                            ConfigManager.Inst.GetHouseConfig(houseConfig.series, houseConfig.level + 1);
                        if (upgradedConfig == null)
                        {
                            return;
                        }

                        upgradeButton.SetActive(true);
                        price.text = upgradedConfig.price.ToString() + "K";
                        upgradeButton.GetComponent<RectTransform>().position =
                            eventData.position + new Vector2(0f, 200f);
                    }
                }
                else
                {
                    ClickHouseData = null;
                    upgradeButton.SetActive(false);
                }
            }
        }
        else if (_state == State.HousePlacement)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
                if (money.counter >= houseConfigs[housepanel._selectedHouseIndex].price)
                {
                    StructureManager.Inst.PlaceHouse(pos.Value);
                    money.counter -= houseConfigs[housepanel._selectedHouseIndex].price;
                }
                else
                {
                    housepanel.log.gameObject.SetActive(true);
                }
            }
        }
        else if (_state == State.Remove)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                PlacementManager.Inst.RemoveObject(pos.Value);
            }
        }
    }

    public void SetState(State state)
    {
        _state = state;
    }

    public void DeselectAllButtons()
    {
        houseButton.Deselect();
        roadButton.Deselect();
        deleteButton.Deselect();
    }

    public void OnUpgradeClick()
    {
        var houseData = ClickHouseData;
        var housePos = houseData.pos;
        var houseConfig = ConfigManager.Inst.GetHouseConfig(houseData.houseId);
        var upgradedConfig = ConfigManager.Inst.GetHouseConfig(houseConfig.series, houseConfig.level + 1);

        if (upgradedConfig != null)
        {
            housepanel.log.gameObject.SetActive(false);
            var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
            if (money.counter >= houseConfigs[housepanel._selectedHouseIndex].price)
            {
                var model = PlacementManager.Inst.GetStructModel(housePos);
                Instantiate(upgradedConfig.prefab, model.transform.position, model.transform.rotation);
                Destroy(model.gameObject);
                DataManager.Inst.SetHouseData(housePos, upgradedConfig.id);
                money.counter -= houseConfigs[housepanel._selectedHouseIndex].price;
                upgradeButton.SetActive(false);
            }
            else
            {
                housepanel.log.gameObject.SetActive(true);
            }
        }
    }

    private Vector3Int? RaycastGround(Vector2 position)
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return Vector3Int.RoundToInt(hit.point);
        }

        return null;
    }
}