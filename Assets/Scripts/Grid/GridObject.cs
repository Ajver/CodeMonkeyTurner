public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private GridOccupant occupant;
    
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

    public void SetOccupant(GridOccupant newOccupant)
    {
        occupant = newOccupant;

        // Try to set helper variables
        occupant.TryGetComponent<Unit>(out unit);
        occupant.TryGetComponent<IInteractable>(out interactable);
        occupant.TryGetComponent<IDamageable>(out damageable);
    }

    public void ClearOccupant()
    {
        occupant = null;
        
        // Clear other helper variables
        unit = null;
        interactable = null;
        damageable = null;
    }
    
    public GridOccupant GetOccupant()
    {
        return occupant;
    }
    
    public bool HasOccupant()
    {
        return occupant != null;
    }

    public bool HasUnit()
    {
        return unit != null;
    }
    
    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public IDamageable GetDamageable()
    {
        return damageable;
    }
    
}
