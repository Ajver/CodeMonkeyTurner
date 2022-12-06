using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] private AudioSource grenadeThrowAudio;

    [SerializeField] private int grenadesLeft = 1;
    
    private enum State
    {
        Aiming,
        Throwing,
    }

    private State state;
    private float stateTimer;
    
    private int throwDistance = 5;
    private Vector3 targetPosition;
    private GridPosition targetGridPosition;
    
    public override string GetActionName()
    {
        return "Grenade";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:
                Vector3 targetPos = targetPosition;
                SlowlyLookAt(targetPos);
                break;
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Throwing;
                float throwingTime = 0.1f;
                stateTimer = throwingTime;

                ThrowGrenade();
                break;
            case State.Throwing:
                break;
        }
    }

    private void ThrowGrenade()
    {
        GrenadeProjectile grenade = Instantiate(grenadeProjectilePrefab, unit.transform.position, Quaternion.identity);
        grenade.Setup(targetGridPosition, OnGrenadeBehaviorComplete);

        grenadeThrowAudio.Play();
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        grenadesLeft--;
        
        state = State.Aiming;
        float aimingTimer = 0.5f;
        stateTimer = aimingTimer;

        targetGridPosition = gridPosition;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        
        ActionStart(callback);  
    }

    private void OnGrenadeBehaviorComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        
        for (int x = -throwDistance; x <= throwDistance; x++)
        {
            for (int z = -throwDistance; z <= throwDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > throwDistance)
                {
                    continue;
                }

                Vector3 testTargetWorldPosition = LevelGrid.Instance.GetWorldPosition(testPos);
                Vector3 unitsDiff = testTargetWorldPosition - unitWorldPosition;
                Vector3 throwDir = unitsDiff.normalized;
                
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        throwDir,
                        unitsDiff.magnitude,
                        obstaclesLayerMask
                    ))
                {
                    // There is an obstacle
                    continue;
                }

                validPosList.Add(testPos);
            }
        }
        
        return validPosList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override bool CanBeTaken()
    {
        return grenadesLeft > 0;
    }
    
    public override int GetAvailableUsagesLeft()
    {
        return grenadesLeft;
    }
    
}
