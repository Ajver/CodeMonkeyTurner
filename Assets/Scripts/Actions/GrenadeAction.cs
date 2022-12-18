using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] private AudioSource grenadeThrowAudio;

    [SerializeField] private int grenadesLeft = 1;
    
    private enum State
    {
        Aiming,
        Throwing,
    }

    private State state;
    private float stateTimer;
    
    private int throwDistance = 5;
    private Vector3 targetPosition;
    private GridPosition targetGridPosition;
    
    public override string GetActionName()
    {
        return "Grenade";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:
                Vector3 targetPos = targetPosition;
                SlowlyLookAt(targetPos);
                break;
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Throwing;
                float throwingTime = 0.1f;
                stateTimer = throwingTime;

                ThrowGrenade();
                break;
            case State.Throwing:
                break;
        }
    }

    private void ThrowGrenade()
    {
        GrenadeProjectile grenade = Instantiate(grenadeProjectilePrefab, unit.transform.position, Quaternion.identity);
        grenade.Setup(targetGridPosition, OnGrenadeBehaviorComplete);

        grenadeThrowAudio.Play();
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        grenadesLeft--;
        
        state = State.Aiming;
        float aimingTimer = 0.5f;
        stateTimer = aimingTimer;

        targetGridPosition = gridPosition;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        
        ActionStart(callback);  
    }

    private void OnGrenadeBehaviorComplete()
    {
        ActionComplete();
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
        
        for (int x = -throwDistance; x <= throwDistance; x++)
        {
            for (int z = -throwDistance; z <= throwDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = gridPosition + offsetGridPos;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > throwDistance)
                {
                    continue;
                }

                Vector3 testTargetWorldPosition = LevelGrid.Instance.GetWorldPosition(testPos);
                Vector3 unitsDiff = testTargetWorldPosition - unitWorldPosition;
                Vector3 throwDir = unitsDiff.normalized;
                
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        throwDir,
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

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Vector3 worldTargetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        float range = GrenadeProjectile.EXPLOSION_RANGE;
        List<IDamageable> damageables =
            ExplosionUtil.Instance.GetDamagableTargetsWithinRange(worldTargetPosition, range);

        int balance = 0;

        // How much hitting barrel is risky. It's added to the final balance
        int barrelRisk = -5;
        
        // We never want to destroy suitcase
        int suitcaseHitValue = -100;
        
        foreach (IDamageable damageable in damageables)
        {
            if (damageable.GetGameTeam() == GameTeam.Neutral)
            {
                // This is Crate or so. Doesn't count to the balance...
                
                if (damageable.GetTransform().TryGetComponent(out ExplodingBarrel barrel))
                {
                    // Better not hit barrel...
                    balance += barrelRisk;
                }
                
                continue;
            }
            
            if (damageable.GetGameTeam() == unit.GetGameTeam())
            {
                // This target is in my team.
                balance--;
            }
            else
            {
                // It's my opponent!
                balance++;
                
                if (damageable.GetTransform().TryGetComponent(out TableWithSuitcase table))
                {
                    // Better not hit barrel...
                    balance += suitcaseHitValue;
                }
            }
        }

        int valuePerUnit = 100;
        
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = balance * valuePerUnit,
        };
    }

    public override bool CanBeTaken()
    {
        return grenadesLeft > 0;
    }
    
    public override int GetAvailableUsagesLeft()
    {
        return grenadesLeft;
    }
    
}
