using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{

    [SerializeField] private Unit unit;
    [SerializeField] private MeshRenderer meshRenderer;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        ShowOrHideIfUnitSelected();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        ShowOrHideIfUnitSelected();
    }

    private void ShowOrHideIfUnitSelected()
    {
        bool isThisUnitSelected = UnitActionSystem.Instance.GetSelectedUnit() == unit;
        meshRenderer.enabled = isThisUnitSelected;
    }
}


