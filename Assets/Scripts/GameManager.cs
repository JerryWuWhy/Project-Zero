using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    
    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;
    public PlacementManager placementManager;

    private void Start()
    {
        inst = this;
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnCancelRoadPlacement += OnCancelSpecialPlacement;
        uiController.OnCancelHousePlacement += OnCancelHousePlacement;
        uiController.OnCancelSpecialPlacement += OnCancelSpecialPlacement;
        uiController.OnDelete += OnDelete;
        uiController.OnCancelDelete += OnCancelDelete;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    public void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }
    private void OnDelete()
    {
        ClearInputActions();
        inputManager.OnMouseClick += placementManager.RemoveObject;
    }

    private void OnCancelSpecialPlacement()
    {
        ClearInputActions();
        inputManager.OnMouseClick -= structureManager.PlaceSpecial;
    }

    private void OnCancelHousePlacement()
    {
        ClearInputActions();
        inputManager.OnMouseClick -= structureManager.PlaceHouse;
    }

    private void OnCancelRoadPlacement()
    {
        ClearInputActions();

        inputManager.OnMouseClick -= roadManager.PlaceRoad;
        inputManager.OnMouseHold -= roadManager.PlaceRoad;
        inputManager.OnMouseUp -= roadManager.FinishPlacingRoad;
    }

    private void OnCancelDelete()
    {
        ClearInputActions();
        inputManager.OnMouseClick -= placementManager.RemoveObject;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }
}
