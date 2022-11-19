using System;
using System.Collections;
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

    public void Spin(Action onSpinComplete)
    {
        isActive = true;
        onActionComplete = onSpinComplete;
    }

}
