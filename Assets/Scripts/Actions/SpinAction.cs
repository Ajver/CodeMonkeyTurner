using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinnedAmount = 0f;

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        totalSpinnedAmount += spinAddAmount;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        if (totalSpinnedAmount >= 360f)
        {
            totalSpinnedAmount = 0f;
            isActive = false;
            onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        isActive = true;
        onActionComplete = onSpinComplete;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }
    
}
