using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform debugObjectPrefab; 
    
    public static LevelGrid Instance { get; private set; }
    
    private GridSystem gridSystem;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one LevelGrid in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(debugObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    { 
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.AddUnit(unit);
    }

    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetUnits();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }
    
}