using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    [SerializeField] private int maxMoveDistance = 4; 
    
    private List<Vector3> positionsList;
    private int currentPositionIndex;

    public override string GetActionName()
    {
        return "Move";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionsList[currentPositionIndex];
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float rotationSpeed = 11f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionsList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }
    
    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        List<GridPosition> gridPositionsPath = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition);

        positionsList = new List<Vector3>();

        foreach (GridPosition gridPos in gridPositionsPath)
        {
            Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(gridPos);
            positionsList.Add(worldPos);
        }
        
        currentPositionIndex = 0;
        
        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(callback);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = unitGridPosition + offsetGridPos;

                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                if (unitGridPosition == testPos)
                {
                    // Cannot move to the same position as already on
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testPos))
                {
                    continue;
                }

                if (!PathFinding.Instance.IsWalkableGridPosition(testPos))
                {
                    // There is an obstacle or so
                    continue;
                }

                int pathLength;
                List<GridPosition> path = PathFinding.Instance.FindPath(unitGridPosition, testPos, out pathLength);
                
                if (path == null)
                {
                    // There spot is unreachable
                    continue;
                }
                
                if (pathLength > maxMoveDistance * 10)
                {
                    // Path is too long
                    continue;
                }
                
                validPosList.Add(testPos);
            }
        }
        
        return validPosList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPos = unit.GetComponent<ShootAction>().GetUnitsCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPos + 1,
        };
    }
}
