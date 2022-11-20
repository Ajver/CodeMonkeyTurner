using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

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
            
        targetUnit.Damage();
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
        ActionStart(callback);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        
        for (int x = -shootDistance; x <= shootDistance; x++)
        {
            for (int z = -shootDistance; z <= shootDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = unitGridPosition + offsetGridPos;
                
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
                
                validPosList.Add(testPos);
            }
        }
        
        return validPosList;
    }
}
