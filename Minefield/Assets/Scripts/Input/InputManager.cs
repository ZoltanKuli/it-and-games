﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    [SerializeField]
    private Camera mainCamera;
    private Vector3 cameraMovementOffset;
    private Vector3 cameraZoomOffset;
    private Vector3 cameraRotationOffset;

    [SerializeField]
    private LayerMask groundMask;

    private Action<Vector3Int> onMouseClickAction;
    private Action<Vector3Int> onMouseHoldAction;
    private Action onMouseUpAction;

    private void Update() {
        InvokeMouseClickActionIfApplicable();
        InvokeMouseUpActionIfApplicable();
        InvokeMouseHoldActionIfApplicable();
        UpdateCameraMovementOffset();
        UpdateCameraZoomOffset();
    }

    /// <summary>
    /// Returns the rounded click position.
    /// </summary>
    private Vector3Int? RaycastGround() {
        RaycastHit raycastHit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, groundMask)) {
            Vector3Int positionInt = Vector3Int.RoundToInt(raycastHit.point);
            return positionInt;
        }

        return null;
    }

    /// <summary>
    /// Invoke mouse click action if applicable.
    /// </summary>
    private void InvokeMouseClickActionIfApplicable() {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false) {
            var position = RaycastGround();
            if (position != null)
                onMouseClickAction?.Invoke(position.Value);
        }
    }

    /// <summary>
    /// Invoke mouse hold action if applicable.
    /// </summary>
    private void InvokeMouseHoldActionIfApplicable() {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false) {
            var position = RaycastGround();
            if (position != null)
                onMouseHoldAction?.Invoke(position.Value);
        }
    }

    /// <summary>
    /// Invoke mouse up action if applicable.
    /// </summary>
    private void InvokeMouseUpActionIfApplicable() {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false) {
            onMouseUpAction?.Invoke();
        }
    }

    /// <summary>
    /// Update camera movement offset.
    /// </summary>
    private void UpdateCameraMovementOffset() {
        cameraMovementOffset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// Update camera zoom offset.
    /// </summary>
    private void UpdateCameraZoomOffset() {
        cameraZoomOffset = new Vector3(0, 0, 0);
        cameraRotationOffset = new Vector3(0, 0, 0);

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            cameraZoomOffset = new Vector3(0, 1, -1);
            cameraRotationOffset = new Vector3(1, 0, 0);
        } else if (0 < Input.GetAxis("Mouse ScrollWheel")) {
            cameraZoomOffset = new Vector3(0, -1, 1);
            cameraRotationOffset = new Vector3(-1, 0, 0);
        }
    }

    /// <summary>
    /// Assign method to on mouse click action.
    /// </summary>
    public void AssignMethodToOnMouseClickAction(Action<Vector3Int> action) {
        onMouseClickAction += action;
    }

    /// <summary>
    /// Assign method to on mouse hold action.
    /// </summary>
    public void AssignMethodToOnMouseHoldAction(Action<Vector3Int> action) {
        onMouseHoldAction += action;
    }

    /// <summary>
    /// Assign method to on mouse up action.
    /// </summary>
    public void AssignMethodToOnMouseUpAction(Action action) {
        onMouseUpAction += action;
    }

    /// <summary>
    /// Reset mouse actions.
    /// </summary>
    public void ResetMouseActions() {
        onMouseClickAction = null;
        onMouseHoldAction = null;
        onMouseUpAction = null;
    }
    
    /// <summary>
    /// Get camera movement offset.
    /// </summary>
    public Vector3 GetCameraMovementOffset() {
        return cameraMovementOffset;
    }
      
    /// <summary>
    /// Get camera zoom offset.
    /// </summary>
    public Vector3 GetCameraZoomOffset() {
        return cameraZoomOffset;
    }
      
    /// <summary>
    /// Get camera rotation offset.
    /// </summary>
    public Vector3 GetCameraRotationOffset() {
        return cameraRotationOffset;
    }
}
