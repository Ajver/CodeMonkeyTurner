using System;
using System.Collections.Generic;
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
    private Unit previousSelectedUnit;

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
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        if (selectedUnit != null)
        {
            SetSelectedUnit(selectedUnit);
        }
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
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
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

        if (unit != null)
        {
            SetSelectedAction(unit.GetAction<MoveAction>());
        }
        else
        {
            SetSelectedAction(null);
        }

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
        if (selectedAction == null)
        {
            return;
        }

        if (InputManager.Instance.IsMouseButtonDownThisFrame())
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

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        if (selectedUnit == sender as Unit)
        {
            DeselectUnit();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            if (previousSelectedUnit == null)
            {
                // The previously selected unit probably died
                // Let's select another one
                List<Unit> allUnits = UnitManager.Instance.GetFriendlyUnitList();
                if (allUnits.Count > 0)
                {
                    Unit firstUnit = allUnits[0];
                    SetSelectedUnit(firstUnit);
                }
            }
            else
            {
                SetSelectedUnit(previousSelectedUnit);
            }
        }
        else
        {
            if (selectedUnit != null)
            {
                previousSelectedUnit = selectedUnit;
                DeselectUnit();   
            }
        }
    }
    
    public void DeselectUnit()
    {
        SetSelectedUnit(null);
    } 
    
}