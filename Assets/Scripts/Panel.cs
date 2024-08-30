using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour
{
    public GameObject panel;  // Reference to the first panel GameObject
    public GameObject panel1; // Reference to the second panel GameObject
    public GameObject panel2; // Reference to the third panel GameObject
    public Image skinImage;
    public List<Sprite> skins = new List<Sprite>();
    public int selectedSkin = 0;
    public StructureManager st;
    public UIController ui;
    
    
    public void NextOption()
    {
        if (skins.Count == 0)
        {
            Debug.LogWarning("Skins list is empty. Cannot change skin.");
            return;
        }
        
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == skins.Count)
        {
            selectedSkin = 0;
        }

        skinImage.sprite = skins[selectedSkin];
    }

    public void BackOption()
    {
        if (skins.Count == 0)
        {
            Debug.LogWarning("Skins list is empty. Cannot change skin.");
            return;
        }
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = skins.Count - 1; // Loop back to the last skin
        }

        skinImage.sprite = skins[selectedSkin];
    }
    
    public void PlayGame()
    {
        // ui.DeselectCurrentButton();
        st.selectedHouseIndex = selectedSkin;
        panel.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    void Start()
    {
        // Ensure all panels are initially hidden
        panel.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf); // Toggle the first panel's active state
        panel1.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel1()
    {
        panel1.SetActive(!panel1.activeSelf); // Toggle the second panel's active state
        panel.SetActive(false);
        panel2.SetActive(false);
    }

    public void TogglePanel2()
    {
        panel2.SetActive(!panel2.activeSelf); // Toggle the third panel's active state
        panel.SetActive(false);
        panel1.SetActive(false);
    }
}
