using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged; 
    public event EventHandler OnSelectedActionChanged; 
    public event EventHandler<bool> OnBusyChanged; 
    public event EventHandler OnActionStarted; 

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;

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

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void SetBusy()
    {
        isBusy = true;
        EmitBusyChangedEvent();
    }

    private void ClearBusy()
    {
        isBusy = false;
        EmitBusyChangedEvent();
    }

    private void EmitBusyChangedEvent()
    {
        OnBusyChanged?.Invoke(this, isBusy);
    }
    
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask);

            if (!hit)
            {
                return false;
            }

            if (hitInfo.collider.TryGetComponent<Unit>(out Unit unit))
            {
                if (unit.IsEnemy())
                {
                    return false;
                }
                
                if (unit == selectedUnit)
                {
                    // Unit already selected
                    return false;
                }

                SetSelectedUnit(unit);
                return true;
            }
        }
        
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);   
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    
}