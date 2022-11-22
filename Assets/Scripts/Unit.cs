using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private bool isEnemy;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    
    private const int ACTION_POINTS_MAX = 2; 

    private MoveAction moveAction;
    private BaseAction[] baseActionsArray;

    private HealthSystem healthSystem;
    
    private GridPosition gridPosition;
    private int actionPoints = ACTION_POINTS_MAX;
    
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        baseActionsArray = GetComponents<BaseAction>();
        
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Start()
    {   
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    
    private void Update()
    {   
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        return baseActionsArray;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (CanSpendActionPointsToTakeAction(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        return false;
    }
    
    public bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        int cost = action.GetActionPointsCost();
        return cost <= actionPoints;
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
        }
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    
}
