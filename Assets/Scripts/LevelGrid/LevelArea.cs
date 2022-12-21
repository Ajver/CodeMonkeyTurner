using System.Collections.Generic;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    
    [SerializeField] private bool isAreaActive;

    private List<Unit> units;
    private Rect boundsRect;

    private void Start()
    {
        LevelGrid.Instance.OnAnyOccupantMovedGridPosition += LevelGrid_OnAnyOccupantMovedGridPosition;

        SetupUnitsInsideBoundary();
    }

    private void SetupUnitsInsideBoundary()
    {
        List<Unit> allUnits = UnitManager.Instance.GetUnitList();

        boundsRect = GetBoundsRect();
        units = new List<Unit>();

        // Loop backwards, because we remove deactivated units from the list
        for (int i = allUnits.Count - 1; i >= 0; i--)
        {
            Unit unit = allUnits[i];
            
            if (IsUnitInside(unit))
            {
                if (!isAreaActive)
                {
                    unit.Deactivate();    
                }
                
                AddUnit(unit);
            }
        }
    }

    private void LevelGrid_OnAnyOccupantMovedGridPosition(object sender, GridOccupant occupant)
    {
        Unit unit = occupant as Unit;
        if (unit == null)
        {
            // It wasn't unit. Let's ignore it then
            return;
        }
        
        bool isUnitInside = IsUnitInside(unit);

        if (units.Contains(unit))
        {
            if (!isUnitInside)
            {
                // Unit just exited the bounding
                units.Remove(unit);
            }
        }
        else if (isUnitInside)
        {
            // We didn't have this unit yet, but now it's inside the bounds!
            AddUnit(unit);
        }
    }

    private bool IsUnitInside(Unit unit)
    {
        Vector3 pos = unit.transform.position;
        return (pos.x >= boundsRect.x &&
                pos.x < boundsRect.x + boundsRect.width &&
                pos.z >= boundsRect.y &&
                pos.z < boundsRect.y + boundsRect.height);
    }
    
    private Rect GetBoundsRect()
    {
        Vector3 position = transform.position;
        Vector3 fullScale = transform.localScale;
        Vector3 halfScale = fullScale * 0.5f;
        
        Rect boundsRect = new Rect(
            position.x - halfScale.x, 
            position.z - halfScale.z, 
            fullScale.x, 
            fullScale.z
            );

        return boundsRect;
    }

    private void AddUnit(Unit unit)
    {
        units.Add(unit);
        unit.SetOccupiedLevelArea(this);
    }
    
    public void Activate()
    {
        if (isAreaActive)
        {
            return;
        }
        
        isAreaActive = true;
        
        foreach (Unit unit in units)
        {
            unit.Activate();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (isAreaActive)
        {
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.5f);     
        }
        else
        {
            Gizmos.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);            
        }
        
        Rect rect = GetBoundsRect();
        Vector3 size = new Vector3(rect.width, 1f, rect.height);
        Vector3 centerPos = new Vector3(rect.x, 1f, rect.y) + size * 0.5f;
        Gizmos.DrawCube(centerPos, size);
    }

    public List<Unit> GetUnitsList()
    {
        return units;
    }

}
