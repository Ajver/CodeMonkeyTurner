using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    public static event EventHandler OnAnySwordHit;
    
    public event EventHandler OnSwordActionStarted; 
    public event EventHandler OnSwordActionCompleted; 

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private State state;
    private float stateTimer;
    
    private int swordDistance = 1;

    private Unit targetUnit;
    
    public override string GetActionName()
    {
        return "Sword";
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        
        ActionStart(callback);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        for (int x = -swordDistance; x <= swordDistance; x++)
        {
            for (int z = -swordDistance; z <= swordDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testPos))
                {
                    continue;
                }

                // targetUnit will NEVER be null, because we checked above if there is a Unit on this position
                Unit testTargetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testPos);
                if (testTargetUnit.IsEnemy() == unit.IsEnemy())
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
            actionValue = 200,
        };
    }

    public void Update()
    {
        if (!isActive)
        {
            return;
        }
        
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDirection = (targetUnit.transform.position - unit.transform.position).normalized;

                float rotationSpeed = 15f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }
        
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = 0.1f;
                stateTimer = afterHitStateTime; 
                
                targetUnit.Damage(100);
                
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }
    
    public int GetMaxSwordDistance()
    {
        return swordDistance;
    }
}
