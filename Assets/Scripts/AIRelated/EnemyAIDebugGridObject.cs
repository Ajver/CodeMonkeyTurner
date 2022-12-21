using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAIDebugGridObject : GridDebugObject
{

    [SerializeField] private Image image;
    [SerializeField] private TextMeshPro valueText;

    private void Awake()
    {
        ClearEnemyAIValue();
    }

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
     
        Testing.Instance.OnEnemyAIActionTestValuesCalculated += Testing_OnEnemyAIActionTestValuesCalculated;
    }

    private void Testing_OnEnemyAIActionTestValuesCalculated(object sender, List<EnemyAIAction> enemyAIActions)
    {
        foreach (EnemyAIAction action in enemyAIActions)
        {
            if (action.gridPosition == gridPosition)
            {
                SetEnemyAIValue(action);
                return;
            }
        }

        // Didn't found action for this position - let's clear itself
        ClearEnemyAIValue();
    }

    private void SetEnemyAIValue(EnemyAIAction action)
    {
        image.enabled = true;
        valueText.enabled = true;

        valueText.text = action.actionValue.ToString();
    }

    private void ClearEnemyAIValue()
    {
        image.enabled = false;
        valueText.enabled = false;
    }
}
