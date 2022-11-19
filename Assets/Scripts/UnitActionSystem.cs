using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged; 

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection())
            {
                return;
            }

            MoveAction moveAction = selectedUnit.GetMoveAction();

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (moveAction.IsValidActionGridPosition(mouseGridPosition))
            {
                moveAction.Move(mouseGridPosition);
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask);

        if (hit)
        {
            if (hitInfo.collider.TryGetComponent<Unit>(out Unit unit))
            {
                Debug.Log("Selected: " + selectedUnit.name);
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