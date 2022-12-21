using System;
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

            Vector3 targetPos = targetPosition;
            SlowlyLookAt(targetPos);

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

                if (!PathFinding.Instance.IsWalkableGridPosition(testPos))
                {
                    // There is an obstacle or so
                    continue;
                }

                if (LevelGrid.Instance.HasOccupantAtGridPosition(testPos))
                {
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
        int targetCountAtGridPos = unit.GetComponent<ShootAction>().GetTargetUnitsCountAtPosition(gridPosition);

        // Used when no target within shoot range, at the target position 
        int defaultValueAction = 20;
        
        int actionValue;

        if (targetCountAtGridPos > 0)
        {
            actionValue = defaultValueAction + targetCountAtGridPos;
        }
        else 
        {
            // No units at this target. Let's find the distance to the closest target anyway,
            // to see how good this direction is

            const float VERY_FAR_DISTANCE = 20f;
            
            float closestDistance = VERY_FAR_DISTANCE;

            Vector3 worldTargetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            
            foreach (Unit testUnit in UnitManager.Instance.GetUnitList())
            {
                if (testUnit.GetGameTeam() == unit.GetGameTeam())
                {
                    // Ignore units in the same team
                    continue;
                }
                
                float dist = Vector3.Distance(worldTargetPosition, testUnit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                }
            }

            // Inverting distance, so the closer it is, the bigger the value is (and the position is worth more)
            float invertedDistance = VERY_FAR_DISTANCE - closestDistance;
            
            // Check how close the closest target was, compared to Very Far Distance
            float ratio = invertedDistance / VERY_FAR_DISTANCE;
            
            // Returns value between 1-20
            // This is mapped based on the How Close Ratio, which is distance in units.
            // Since we use invertedDistance, the closer it is, the bigger ratio is (and then the mapped value)
            float mappedValueF = Mathf.Lerp(1f, defaultValueAction, ratio);
            actionValue = Mathf.RoundToInt(mappedValueF);
        }

        List<Door> doorsNextToIt = GetDoorsNextToGridPosition(gridPosition);

        foreach (Door door in doorsNextToIt)
        {
            // For each door next to this position, increase the final actionValue by the HALF value of opening the door
            actionValue += AIBrain.GetOpenDoorActionValue(door, unit) / 2;
        }

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = actionValue,
        };
    }

    private List<Door> GetDoorsNextToGridPosition(GridPosition gridPosition)
    {
        List<Door> doors = new List<Door>();

        int distance = InteractAction.INTERACT_DISTANCE;
        
        for (int x = -distance; x <= distance; x++)
        {
            for (int z = -distance; z <= distance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                GridOccupant occupant = LevelGrid.Instance.GetOccupantAtGridPosition(testPos);
                Door door = occupant as Door;
                
                if (door != null)
                {
                    doors.Add(door);
                }
            }
        }
        
        return doors;
    }

}
