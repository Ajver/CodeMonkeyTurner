using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action callback);

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionsList = GetValidActionGridPositionList();
        return validGridPositionsList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }
    
    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();

        List<GridPosition> validGridPositions = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validGridPositions)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActions.Add(enemyAIAction);
        }

        if (enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActions[0];
        }
        else
        {
            // No possible Enemy AI actions
            return null;
        }
    }
    
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
