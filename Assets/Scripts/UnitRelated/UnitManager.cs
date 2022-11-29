using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public event EventHandler OnAllFriendlyUnitsDied;
    public event EventHandler OnAllEnemyUnitsDied;

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one UnitManager in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        unitList.Add(unit);
        
        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        unitList.Remove(unit);
        
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);

            if (enemyUnitList.Count <= 0)
            {
                OnAllEnemyUnitsDied?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            friendlyUnitList.Remove(unit);
            
            if (friendlyUnitList.Count <= 0)
            {
                OnAllFriendlyUnitsDied?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
    
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }
}
