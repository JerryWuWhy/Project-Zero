using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
    private Vector2 cameraMovementVector;

    [SerializeField] Camera mainCamera;

    public LayerMask groundMask;

    public Vector2 CameraMovementVector
    {
        get { return cameraMovementVector; }
    }

    private void Update()
    {
#if UNITY_IOS || UNITY_ANDROID

        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Loop through all touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Get the touch
                Touch touch = Input.GetTouch(i);

                // Handle different touch phases
                var position = RaycastGround(touch.position);
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        if (position != null)
                            OnMouseClick?.Invoke(position.Value);
                        break;
                    case TouchPhase.Moved:
                        // Touch moved
                        if (position != null)
                            OnMouseHold?.Invoke(position.Value);
                        break;
                    case TouchPhase.Ended:
                        // Touch ended
                        OnMouseUp?.Invoke();
                        break;
                }
            }
        }
#else
		CheckClickDownEvent();
		CheckClickUpEvent();
		CheckClickHoldEvent();
		CheckArrowInput();
#endif
    }

    private Vector3Int? RaycastGround(Vector2 position)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            return positionInt;
        }

        return null;
    }

    private void CheckArrowInput()
    {
        cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void CheckClickHoldEvent()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround(Input.mousePosition);
            if (position != null)
                OnMouseHold?.Invoke(position.Value);
        }
    }

    private void CheckClickUpEvent()
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            OnMouseUp?.Invoke();
        }
    }

    private void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround(Input.mousePosition);
            if (position != null)
                OnMouseClick?.Invoke(position.Value);
        }
    }
}