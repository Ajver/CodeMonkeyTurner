using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{

    private int interactDistance = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(callback);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        for (int x = -interactDistance; x <= interactDistance; x++)
        {
            for (int z = -interactDistance; z <= interactDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasInteractableAtGridPosition(testPos))
                {
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
