using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUnitOutOfActionPointsUI : MonoBehaviour
{
    
    public void OnNextUnitBtnPressed()
    {
        foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList())
        {
            if (unit.GetActionPoints() > 0)
            {
                UnitActionSystem.Instance.SetSelectedUnit(unit, true);
                return;
            }
        }
    }

}
