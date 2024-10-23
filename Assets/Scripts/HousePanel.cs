using System;
using UnityEngine;
using UnityEngine.UI;

public class HousePanel : MonoBehaviour
{
    public Image skinImage;

    private int _selectedHouseIndex;

    public void OnNextClick()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex + 1) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
    }

    public void OnPrevClick()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex - 1 + houseConfigs.Count) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
    }

    public void OnHouseClick()
    {
        StructureManager.Inst.selectedHouseIndex = _selectedHouseIndex;
        gameObject.SetActive(false);
        Hud.Inst.SetState(Hud.State.HousePlacement);
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Hud.Inst.SetState(Hud.State.None);
        Hud.Inst.DeselectAllButtons();
    }
}