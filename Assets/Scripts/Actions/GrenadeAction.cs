using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    
    private int throwDistance = 7;
    
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
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        GrenadeProjectile grenade = Instantiate(grenadeProjectilePrefab, unit.transform.position, Quaternion.identity);
        grenade.Setup(gridPosition, OnGrenadeBehaviorComplete);
        
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
}
