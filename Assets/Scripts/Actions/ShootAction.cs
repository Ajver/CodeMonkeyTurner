using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    [SerializeField] private LayerMask obstaclesLayerMask;
    
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    } 
    
    [SerializeField] private int shootDistance = 7;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private float stateTimer;

    private Unit targetUnit;
    private bool canShootBullet;
    
    public override string GetActionName()
    {
        return "Shoot";
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
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                TryShoot();
                break;
            case State.Cooloff:
                break;
        }
        
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Aim()
    {
        Vector3 faceDirection = (targetUnit.transform.position - unit.transform.position).normalized;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, faceDirection, rotationSpeed * Time.deltaTime);
    }
    
    private void TryShoot()
    {
        if (canShootBullet)
        {
            canShootBullet = false;
            Shoot();
        }
    }
    
    private void Shoot()
    {
        OnShootEventArgs eventArgs = new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit,
        };
        OnShoot?.Invoke(this, eventArgs);

        int damageAmount = 40;
        targetUnit.Damage(damageAmount);
    }
    
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                canShootBullet = true;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }
    
    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        
        ActionStart(callback);
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
        
        for (int x = -shootDistance; x <= shootDistance; x++)
        {
            for (int z = -shootDistance; z <= shootDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > shootDistance)
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

                Vector3 testTargetWorldPosition = testTargetUnit.transform.position;
                Vector3 unitsDiff = testTargetWorldPosition - unitWorldPosition;
                Vector3 shootDir = unitsDiff.normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                    unitWorldPosition + Vector3.up * unitShoulderHeight,
                    shootDir,
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

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return shootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        // The lower HP unit has, the bigger the score is (so it's more attractive to be shoot) 
        int unitWeaknessScore = Mathf.RoundToInt((1 - unit.GetHealthNormalized()) * 100f);
        
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + unitWeaknessScore,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        List<GridPosition> targetsPositions = GetValidActionGridPositionList();
        return targetsPositions.Count;
    }
}
