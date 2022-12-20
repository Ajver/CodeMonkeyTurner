using System;
using UnityEngine;

public class Unit : GridOccupant, IDamageable
{

    [SerializeField] private bool isEnemy;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    public static event EventHandler OnAnyUnitDamaged;
    public static event EventHandler OnAnyUnitActivated;
    public static event EventHandler OnAnyUnitDeactivated;
    
    private const int ACTION_POINTS_MAX = 2; 

    private BaseAction[] baseActionsArray;

    private HealthSystem healthSystem;
    
    private int actionPoints = ACTION_POINTS_MAX;
    
    private void Awake()
    {
        baseActionsArray = GetComponents<BaseAction>();
        
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    protected override void OccupantStart()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    protected override void OccupantUpdate()
    {
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionsArray)
        {
            if (baseAction is T)
            {
                return baseAction as T;
            }
        }

        return null;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        return baseActionsArray;
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
        OnAnyUnitDamaged?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
        ClearItselfFromGrid();
        
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() => healthSystem.GetHealthNormalized();
    
    public GameTeam GetGameTeam()
    {
        if (isEnemy)
        {
            return GameTeam.Enemy;
        }
        else
        {
            return GameTeam.Player;
        }
    }
    
    public Transform GetTransform()
    {
        return transform;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        OnAnyUnitActivated?.Invoke(this, EventArgs.Empty);
        AddItselfToGrid();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnAnyUnitDeactivated?.Invoke(this, EventArgs.Empty);
        ClearItselfFromGrid();
    }
}
