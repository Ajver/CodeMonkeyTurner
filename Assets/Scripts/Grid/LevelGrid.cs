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
    
    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    { 
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.SetUnit(unit);
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.ClearUnit();
    }
    
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        if (GetUnitAtGridPosition(fromGridPosition) == unit)
        {
            ClearUnitAtGridPosition(fromGridPosition);
            ClearDamageableAtGridPosition(fromGridPosition);
        }

        if (!HasUnitOnGridPosition(toGridPosition))
        {
            SetUnitAtGridPosition(toGridPosition, unit);
            SetDamageableAtGridPosition(toGridPosition, unit);
        }

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
    
    public bool HasDamageableOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetDamageable() != null;
    }

    public void SetDamageableAtGridPosition(GridPosition gridPosition, IDamageable damageable)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.SetDamageable(damageable);
    }
    
    public IDamageable GetDamageableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetDamageable();
    }

    public void ClearDamageableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.ClearDamageable();
    }

    
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition)  => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.HasUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject obj = gridSystem.GetGridObject(gridPosition);
        return obj.GetUnit();
    }
    
    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

}
