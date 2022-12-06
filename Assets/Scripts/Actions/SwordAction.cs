using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{

    public static event EventHandler OnAnySwordHit;
    
    public event EventHandler OnSwordActionStarted; 
    public event EventHandler OnSwordActionCompleted;

    [SerializeField] private AudioSource swordSwingAudio;
    [SerializeField] private AudioSource swordHitAudio;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private State state;
    private float stateTimer;
    
    private int swordDistance = 1;

    private IDamageable damageableTarget;
    
    public override string GetActionName()
    {
        return "Sword";
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        swordSwingAudio.Play();
        
        damageableTarget = LevelGrid.Instance.GetDamageableAtGridPosition(gridPosition);
        
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

                if (!LevelGrid.Instance.HasDamageableOnGridPosition(testPos))
                {
                    continue;
                }

                // targetUnit will NEVER be null, because we checked above if there is a Unit on this position
                IDamageable testDamageableTarget = LevelGrid.Instance.GetDamageableAtGridPosition(testPos);
                if (testDamageableTarget.GetGameTeam() == unit.GetGameTeam())
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
        if (LevelGrid.Instance.HasUnitOnGridPosition(gridPosition))
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 200,
            };
        }
        else
        {
            // Attacking non-Unit is stupid idea. Don't do this.
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }
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
                SlowlyLookAt(damageableTarget.GetTransform().position);
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

                SwordHit();
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void SwordHit()
    {
        damageableTarget.Damage(100);

        swordHitAudio.Play();
        
        OnAnySwordHit?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetMaxSwordDistance()
    {
        return swordDistance;
    }
}
