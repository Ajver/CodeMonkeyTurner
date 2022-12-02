using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    [SerializeField] private float unitRotationSpeed = 5f;
    
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected void SlowlyLookAt(Vector3 targetPos)
    {
        Vector3 faceDirection = (targetPos - unit.transform.position).normalized;
        faceDirection.y = 0f;

        float diff = Vector3.Distance(faceDirection, transform.forward);
        
        // The more unit has to rotate, the faster it does it
        float rotationSpeed = unitRotationSpeed * diff;
        rotationSpeed = Mathf.Max(rotationSpeed, 5f);
        transform.forward = Vector3.Lerp(transform.forward, faceDirection, rotationSpeed * Time.deltaTime);
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

    protected void OnDestroy()
    {
        if (isActive)
        {
            ActionComplete();
        }
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
