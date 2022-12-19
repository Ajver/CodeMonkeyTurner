using System;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    public static Testing Instance;

    public event EventHandler OnTestWinMission; 
    public event EventHandler OnTestLoseMission;

    public event EventHandler<List<EnemyAIAction>> OnEnemyAIActionTestValuesCalculated;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    }

    public void TestWinMission()
    {
        OnTestWinMission?.Invoke(this, EventArgs.Empty);
    }
    
    public void TestLoseMission()
    {
        OnTestLoseMission?.Invoke(this, EventArgs.Empty);
    }
    
    // private void Update()
    // {
    //     if (InputManager.Instance.IsTestActionPressedThisFrame())
    //     {
    //         Debug.Log("TEST ACTION PRESSED!");
    //     }
    // }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        HandleTestEnemyAIAction();
    }
    
    private void HandleTestEnemyAIAction()
    {
        if (UnitManager.Instance.GetEnemyUnitList().Count == 0)
        {
            Debug.Log("No enemies found...");
            return;
        }

        BaseAction action = UnitActionSystem.Instance.GetSelectedAction();
        if (action == null)
        {
            Debug.Log("No action selected");
            return;
        }
        
        List<EnemyAIAction> enemyAIActions = action.GetAllPossibleEnemyAIActions();
        
        OnEnemyAIActionTestValuesCalculated?.Invoke(this, enemyAIActions);
    }
    
    public static bool IsTestingEnvironment()
    {
        return Instance != null;
    }

}
