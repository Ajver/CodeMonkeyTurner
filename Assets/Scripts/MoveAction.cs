using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private int maxMoveDistance = 4; 

    private Animator unitAnimator;
    private Vector3 targetPosition;

    private Unit unit;
    
    private void Awake()
    {
        unit = GetComponent<Unit>();
        unitAnimator = unit.GetAnimator();
        targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float rotationSpeed = 11f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotationSpeed * Time.deltaTime);

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
    }
    
    public void Move(GridPosition gridPosition)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionsList = GetValidActionGridPositionList();
        return validGridPositionsList.Contains(gridPosition);
    }
    
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPosList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testPos = unitGridPosition + offsetGridPos;

                if (!LevelGrid.Instance.IsValidGridPosition(testPos))
                {
                    continue;
                }

                if (unit.GetGridPosition() == testPos)
                {
                    // Cannot move to the same position as already on
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testPos))
                {
                    continue;
                }
                
                validPosList.Add(testPos);
            }
        }
        
        return validPosList;
    }

}
