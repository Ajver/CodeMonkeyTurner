using UnityEngine;

public abstract class GridOccupant : MonoBehaviour
{
    [SerializeField] protected bool isWalkable = false;

    protected GridPosition gridPosition;

    private LevelArea occupiedLevelArea;

    protected void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        AddItselfToGrid();

        OccupantStart();
    }

    protected abstract void OccupantStart();
    
    private void Update()
    {   
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            
            gridPosition = newGridPosition;
            LevelGrid.Instance.OccupantMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
        
        OccupantUpdate();
    }

    protected abstract void OccupantUpdate();
    
    protected void AddItselfToGrid()
    {
        LevelGrid.Instance.SetOccupantAtGridPosition(gridPosition, this);
    }
    
    protected void ClearItselfFromGrid()
    {
        LevelGrid.Instance.ClearOccupantAtGridPosition(gridPosition);
    }
    
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetOccupiedLevelArea(LevelArea area)
    {
        Debug.Log($"{gameObject} changed area from {occupiedLevelArea} to {area}");
        occupiedLevelArea = area;
    }

    public LevelArea GetOccupiedLevelArea(LevelArea area)
    {
        return occupiedLevelArea;
    }

    public bool IsOccupyingLevelArea(LevelArea area)
    {
        return occupiedLevelArea == area;
    }
    
}
