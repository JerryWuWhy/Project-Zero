using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnHousePlacement, OnSpecialPlacement, OnDelete;
    public Action OnCancelRoadPlacement, OnCancelHousePlacement, OnCancelSpecialPlacement, OnCancelDelete;
    public Button placeRoadButton, placeHouseButton, placeSpecialButton, deleteButton;
    public Color outlineColor;
    private List<Button> buttonList;
    public Button currentlySelectedButton = null;

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton };

        placeRoadButton.onClick.AddListener(() => OnButtonClick(placeRoadButton, OnRoadPlacement, OnCancelRoadPlacement));
        placeHouseButton.onClick.AddListener(() => OnButtonClick(placeHouseButton, OnHousePlacement, OnCancelHousePlacement));
        placeSpecialButton.onClick.AddListener(() => OnButtonClick(placeSpecialButton, OnSpecialPlacement, OnCancelSpecialPlacement));
        deleteButton.onClick.AddListener(() => OnButtonClick(deleteButton, OnDelete, OnCancelDelete));
    }

    public void DeselectCurrentButton(){
        // If the button is already selected, deselect it and cancel the action
        DeselectButton(currentlySelectedButton);
        currentlySelectedButton = null;
        OnCancelDelete?.Invoke();
    }

    public void OnButtonClick(Button button, Action placementAction, Action cancelAction)
    {
        if (currentlySelectedButton == button)
        {
            // If the button is already selected, deselect it and cancel the action
            DeselectButton(button);
            currentlySelectedButton = null;
            cancelAction?.Invoke(); // Invoke the cancel action
        }
        else
        {
            // Select new button
            ResetButtonColor();
            ModifyOutline(button);
            currentlySelectedButton = button;
            placementAction?.Invoke();
            
        }
    }
    
    public void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    public void DeselectButton(Button button)
    {
        button.GetComponent<Outline>().enabled = false;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}