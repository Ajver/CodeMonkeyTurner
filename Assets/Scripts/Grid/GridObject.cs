using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private Unit unit;
    private IInteractable interactable;
    private IDamageable damageable;
    
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition + "\n" + unit;
    }

    public void SetUnit(Unit newUnit)
    {
        if (unit != null)
        {
            // Some unit is already standing here.
            // The other unit is probably just moving through this place
            return;
        }

        unit = newUnit;
    }

    public void ClearUnit()
    {
        unit = null;
    }
    
    public Unit GetUnit()
    {
        return unit;
    }
    
    public bool HasUnit()
    {
        return unit != null;
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable inter)
    {
        interactable = inter;
    }

    public void ClearInteractable()
    {
        interactable = null;
    }

    public IDamageable GetDamageable()
    {
        return damageable;
    }
    
    public void SetDamageable(IDamageable hitt)
    {
        damageable = hitt;
    }

    public void ClearDamageable()
    {
        damageable = null;
    }
    
}
