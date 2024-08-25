using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    public GameObject panel;  // Reference to the first panel GameObject
    public GameObject panel1; // Reference to the second panel GameObject
    public GameObject panel2; // Reference to the third panel GameObject
    public SpriteRenderer sr;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;
    public GameObject houseskin;

    public void NextOption()
    {
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == skins.Count)
        {
            selectedSkin = 0;
        }

        sr.sprite = skins[selectedSkin];
    }
    public void BackOption()
    {
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = selectedSkin - 1;
        }

        sr.sprite = skins[selectedSkin];
    }

    public void PlayGame()
    {
        
        
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
    }

    public void TogglePanel1()
    {
        panel1.SetActive(!panel1.activeSelf); // Toggle the second panel's active state
    }

    public void TogglePanel2()
    {
        panel2.SetActive(!panel2.activeSelf); // Toggle the third panel's active state
    }
}
