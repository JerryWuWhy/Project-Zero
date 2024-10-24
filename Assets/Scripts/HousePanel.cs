using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class HousePanel : MonoBehaviour
{
    public TextMeshProUGUI log;
    public Image skinImage;
    public TextMeshProUGUI price;
    public int _selectedHouseIndex;
    public Money money;
    public void OnNextClick()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex + 1) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
        price.text =  houseConfigs[_selectedHouseIndex].price.ToString()+" K";
    }

    public void OnPrevClick()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex - 1 + houseConfigs.Count) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
        price.text =  houseConfigs[_selectedHouseIndex].price.ToString()+" K";
    }

    public void OnHouseClick()
    {
        log.gameObject.SetActive(false);
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