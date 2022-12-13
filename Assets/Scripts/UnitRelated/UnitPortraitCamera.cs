using System;
using UnityEngine;

public class UnitPortraitCamera : MonoBehaviour
{
    private Unit unit;
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        unit = UnitActionSystem.Instance.GetSelectedUnit();

        bool shouldBeActive = unit != null;
        gameObject.SetActive(shouldBeActive);
    }

    private void LateUpdate()
    {
        if (unit == null)
        {
            return;
        }
        
        Transform unitTransform = unit.transform;
        transform.SetPositionAndRotation(unitTransform.position, unitTransform.rotation);
    }
}
