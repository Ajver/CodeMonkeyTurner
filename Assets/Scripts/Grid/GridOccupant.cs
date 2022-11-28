using System;
using UnityEngine;

public abstract class GridOccupant : MonoBehaviour
{

    protected GridPosition gridPosition;

    protected void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetOccupantAtGridPosition(gridPosition, this);

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
    
    protected void OnDestroy()
    {
        LevelGrid.Instance.ClearOccupantAtGridPosition(gridPosition);
    }
    
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
