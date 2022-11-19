using System;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged; 

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool isBusy;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of UnitActionSystem! From: " + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!TryHandleUnitSelection())
            {
                MoveAction moveAction = selectedUnit.GetMoveAction();

                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                if (moveAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    SetBusy();
                    moveAction.Move(mouseGridPosition, ClearBusy);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    {
        isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask);

        if (hit)
        {
            if (hitInfo.collider.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    
}