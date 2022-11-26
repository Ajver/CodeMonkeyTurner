using System.Collections.Generic;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> units;
    private Door door;
    
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;

        units = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";

        foreach (Unit unit in units)
        {
            unitString += "\n" + unit;
        }
        
        return gridPosition + unitString;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }
    
    public List<Unit> GetUnits()
    {
        return units;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return units[0];
        }

        return null;
    }

    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }

    public Door GetDoor()
    {
        return door;
    }

    public void SetDoor(Door newDoor)
    {
        door = newDoor;
    }
    
}
