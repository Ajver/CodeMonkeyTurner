using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;

    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] private AudioSource aimAudio;
    [SerializeField] private AudioSource shootAudio;
    
    public class OnShootEventArgs : EventArgs
    {
        public IDamageable DamageableTarget;
        public Unit shootingUnit;
    } 
    
    [SerializeField] private int shootDistance = 8;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private float stateTimer;

    private IDamageable damageableTarget;
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
        Vector3 targetPos = damageableTarget.GetTransform().position;
        SlowlyLookAt(targetPos);
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
            DamageableTarget = damageableTarget,
            shootingUnit = unit,
        };
        OnShoot?.Invoke(this, eventArgs);
        OnAnyShoot?.Invoke(this, eventArgs);
        
        shootAudio.Play();
        
        int damageAmount = 50;
        damageableTarget.Damage(damageAmount);
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
        damageableTarget = LevelGrid.Instance.GetDamageableAtGridPosition(gridPosition);
        
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        
        aimAudio.Play();
        
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
                
                if (!LevelGrid.Instance.HasDamageableOnGridPosition(testPos))
                {
                    continue;
                }

                IDamageable testDamageableTarget = LevelGrid.Instance.GetDamageableAtGridPosition(testPos);
                if (testDamageableTarget.GetGameTeam() == unit.GetGameTeam())
                {
                    continue;
                }

                Vector3 testTargetWorldPosition = testDamageableTarget.GetTransform().position;
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

    public IDamageable GetDamageableTarget()
    {
        return damageableTarget;
    }

    public int GetMaxShootDistance()
    {
        return shootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        IDamageable damageable = LevelGrid.Instance.GetDamageableAtGridPosition(gridPosition);

        switch (damageable) {
            case Unit unit:
                // The lower HP unit has, the bigger the score is (so it's more attractive to be shoot) 
                int unitWeaknessScore = Mathf.RoundToInt((1 - unit.GetHealthNormalized()) * 100f);
            
                return new EnemyAIAction
                {
                    gridPosition = gridPosition,
                    actionValue = 100 + unitWeaknessScore,
                };
            // TODO: Count how many enemies will this kill (excluding case when it kills allies too)
            //  and decide if shoot or not
            // case ExplodingBarrel barrel:
            //     return new EnemyAIAction
            //     {
            //         gridPosition = gridPosition,
            //         actionValue = 0,
            //     };
            default:
                // Not unit, neither Exploding barrel. Very unattractive target.
                return new EnemyAIAction
                {
                    gridPosition = gridPosition,
                    actionValue = 0,
                };
        }
    }

    public int GetUnitsCountAtPosition(GridPosition gridPosition)
    {
        List<GridPosition> targetsPositions = GetValidActionGridPositionList(gridPosition);

        int unitsCount = 0;
        foreach (GridPosition testPos in targetsPositions)
        {
            if (LevelGrid.Instance.HasUnitOnGridPosition(testPos))
            {
                unitsCount++;
            }
        }
        
        return unitsCount;
    }
}
