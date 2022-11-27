using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public event EventHandler OnAnyUnitMovedGridPosition;
    
    [SerializeField] private Transform debugObjectPrefab; 
    
    public static LevelGrid Instance { get; private set; }
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    
    private GridSystem<GridObject> gridSystem;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one LevelGrid in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (gs, position) => new GridObject(gs, position));
        // gridSystem.CreateDebugObjects(debugObjectPrefab);
    }

    private void Start()
    {
        PathFinding.Instance.Setup(width, height, cellSize);
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
    
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetInteractable();
    }

    public bool HasInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetInteractable() != null;
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.SetInteractable(interactable);
    }

    public void ClearInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.ClearInteractable();
    }
    
    public bool HasShootableOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetShootable() != null;
    }

    public void SetShootableAtGridPosition(GridPosition gridPosition, IShootable shootable)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.SetShootable(shootable);
    }
    
    public IShootable GetShootableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetShootable();
    }

    public void ClearShootableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.ClearShootable();
    }

    
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition)  => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.GetUnit();
    }
    
    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

}
