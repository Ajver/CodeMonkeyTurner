using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    
    [SerializeField] private bool isAreaActive;

    [SerializeField] private Door[] doorsWhichActivates;

    private List<Unit> units;
    private Rect boundsRect;
    
    private void Start()
    {
        boundsRect = GetBoundsRect();
        units = new List<Unit>();

        List<Unit> allEnemyUnits = UnitManager.Instance.GetEnemyUnitList();

        // Loop backwards, because we remove units from the list 
        for (int i = allEnemyUnits.Count - 1; i >= 0; i--)
        {
            Unit unit = allEnemyUnits[i];
            
            if (IsUnitInside(unit))
            {
                if (!isAreaActive)
                {
                    unit.Deactivate();    
                }
                
                units.Add(unit);
            }
        }

        foreach (Door door in doorsWhichActivates)
        {
            door.OnOpened += OnDoorWhichActivateOpened;
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

    private void OnDoorWhichActivateOpened(object sender, EventArgs e)
    {
        if (isAreaActive)
        {
            return;
        }
        
        ActivateArea();
    }
    
    private void ActivateArea()
    {
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
    
}
