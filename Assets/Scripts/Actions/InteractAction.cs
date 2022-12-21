using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private enum State
    {
        LookingInto,
        Interacting,
    }

    private State state;
    private float stateTimer;

    private IInteractable interactable;
    
    public const int INTERACT_DISTANCE = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.LookingInto:
                SlowlyLookAt(interactable.GetTransform().position);
                break;
            case State.Interacting:
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
            case State.LookingInto:
                state = State.Interacting;
                interactable.Interact(OnInteractComplete);

                // Don't need any state timer - the interaction time is controlled by the Interactable object 
                isActive = false;
                break;
            case State.Interacting:
                break;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        
        state = State.LookingInto;
        float lookingIntoTime = .4f;
        
        Vector3 faceDirection = (interactable.GetTransform().position - unit.transform.position).normalized;
        faceDirection.y = 0f;
        float diff = Vector3.Distance(faceDirection, transform.forward);
        
        // The more Unit needs to rotate to the target, the longer timer is set
        stateTimer = lookingIntoTime * diff;
        
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

        for (int x = -INTERACT_DISTANCE; x <= INTERACT_DISTANCE; x++)
        {
            for (int z = -INTERACT_DISTANCE; z <= INTERACT_DISTANCE; z++)
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
        int actionValue = 0;
        
        Door door = LevelGrid.Instance.GetOccupantAtGridPosition(gridPosition) as Door;

        if (door != null)
        {
            actionValue += AIBrain.GetOpenDoorActionValue(door, unit);
        }
        
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = actionValue,
        };
    }
}
