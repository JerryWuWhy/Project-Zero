using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel; 
    public GameObject panel1; 
    public GameObject panel2; 

    public Image skinImage;

    public ConfigManager cf;
    public StructureManager st;
    public UIController ui;

    public int _selectedHouseIndex;

    public void NextOption()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex + 1) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
    }

    public void BackOption()
    {
        var houseConfigs = ConfigManager.Inst.GetHouseConfigsByLevel(1);
        _selectedHouseIndex = (_selectedHouseIndex - 1 + houseConfigs.Count) % houseConfigs.Count;
        skinImage.sprite = houseConfigs[_selectedHouseIndex].image;
    }

    public void PlayGame()
    {
        st.selectedHouseIndex = _selectedHouseIndex;
        panel.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    void Start()
    {
        panel.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel1()
    {
        panel1.SetActive(!panel1.activeSelf); 
        panel.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel2()
    {
        panel2.SetActive(!panel2.activeSelf); 
        panel.SetActive(false);
        panel1.SetActive(false);
    }
}