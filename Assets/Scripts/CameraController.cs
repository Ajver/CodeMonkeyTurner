using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private const float MIN_FOLLOW_Y_OFFSET = 1.0f;
    private const float MAX_FOLLOW_Y_OFFSET = 12.0f;

    private CinemachineTransposer transposer;
    private Vector3 targetFollowOffset;

    private bool isMovingToTarget;
    private Vector3 targetPosition;
    
    private void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = transposer.m_FollowOffset;
    }

    private void Start()
    {
        GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
        UnitActionSystem.Instance.OnFocusOnSelectedUnitRequested += UnitActionSystem_OnFocusOnSelectedUnitRequested;
    }

    private void GameManager_OnGameEnded(object sender, EventArgs e)
    {
        // Disables controls
        enabled = false;
    }

    private void UnitActionSystem_OnFocusOnSelectedUnitRequested(object sender, EventArgs e)
    {
        FocusOnSelectedUnit();
    }

    private void FocusOnSelectedUnit()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();

        if (unit == null)
        {
            return;
        }
        
        targetPosition = unit.transform.position;
        targetPosition.y = transform.position.y;
        isMovingToTarget = true;
    }
    
    void Update()
    {
        HandleFocusing();
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleFocusing()
    {
        if (InputManager.Instance.IsFocusUnitBtnPressed())
        {
            FocusOnSelectedUnit();
        }
    }
    
    private void HandleMovement()
    {
        if (isMovingToTarget)
        {
            Vector3 currentPos = transform.position;
            Vector2 diff = new Vector2(currentPos.x, currentPos.z) - new Vector2(targetPosition.x, targetPosition.z);
            if (diff.sqrMagnitude < 0.1f)
            {
                isMovingToTarget = false;
                return;
            }
            
            float moveSpeed = 15f;
            transform.position = Vector3.Lerp(currentPos, targetPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
            Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            
            float moveSpeed = 10f;
            transform.position += moveVector.normalized * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float transitionSpeed = 7.0f * Time.deltaTime;
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, transitionSpeed);
    }
    
}
