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

    public GameObject upgradeButton;
    public GameObject priceLabel;
    public TMP_Text priceText;

    public enum State
    {
        None,
        RoadPlacement,
        HousePlacement,
        Remove,
        Upgrade
    }

    public static Hud Inst { get; private set; }

    private State _state;
    private Vector3Int _clickedPos;

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
        if (_state == State.None)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                var houseData = DataManager.Inst.GetHouseData(pos.Value);
                if (houseData != null)
                {
                    _clickedPos = pos.Value;
                    upgradeButton.SetActive(true);
                    upgradeButton.GetComponent<RectTransform>().position = eventData.position + new Vector2(0f, 200f);
                }
                else
                {
                    upgradeButton.SetActive(false);
                }
            }
        }
        else if (_state == State.HousePlacement)
        {
            var pos = RaycastGround(eventData.position);
            if (pos.HasValue)
            {
                StructureManager.Inst.PlaceHouse(pos.Value);
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
        var houseData = DataManager.Inst.GetHouseData(_clickedPos);
        var houseConfig = ConfigManager.Inst.GetHouseConfig(houseData.houseId);
        var upgradedConfig = ConfigManager.Inst.GetHouseConfig(houseConfig.series, houseConfig.level + 1);

        if (upgradedConfig != null)
        {
            var model = PlacementManager.Inst.GetStructModel(_clickedPos);
            Instantiate(upgradedConfig.prefab, model.transform.position, model.transform.rotation);
            Destroy(model.gameObject);
            DataManager.Inst.SetHouseData(_clickedPos, upgradedConfig.id);

            upgradeButton.SetActive(false);
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