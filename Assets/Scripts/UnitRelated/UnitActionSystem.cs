using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged; 
    public event EventHandler OnFocusOnSelectedUnitRequested; 
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
            SetSelectedUnit(selectedUnit, false);
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
            return TrySelectUnitByMouse();
        }
        
        return TryChangeSelectedUnit();
    }

    private bool TrySelectUnitByMouse()
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

            SetSelectedUnit(unit, false);
            return true;
        }

        return false;
    }

    private bool TryChangeSelectedUnit()
    {
        int dir = InputManager.Instance.GetChangeSelectedUnitAxisThisFrame();

        if (dir == 0)
        {
            return false;
        }

        ChangeSelectedUnit(dir);
        return true;
    }

    public void ChangeSelectedUnit(int dir)
    {
        List<Unit> units = UnitManager.Instance.GetFriendlyUnitList();
        int count = units.Count; 
        if (count <= 0)
        {
            return;
        }

        int currentUnitIdx = 0;

        if (selectedUnit != null)
        {
            for (int i = 0; i < count; i++)
            {
                if (units[i] == selectedUnit)
                {
                    currentUnitIdx = i;
                    break;
                }
            }
        }
        
        // Add extra count, to always have non-negative values 
        int anotherUnitIdx = (currentUnitIdx + dir + count) % count;
        Unit anotherUnit = units[anotherUnitIdx];

        if (anotherUnit != selectedUnit)
        {
            SetSelectedUnit(anotherUnit, true);
        }
    }
    
    public void SetSelectedUnit(Unit unit, bool focusOnUnit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

        if (unit != null)
        {
            SetSelectedAction(unit.GetAction<MoveAction>());
        }
        else
        {
            SetSelectedAction(null);
        }

        if (focusOnUnit && unit != null)
        {
            FocusOnUnit();
        }
    }

    public void FocusOnUnit()
    {
        OnFocusOnSelectedUnitRequested?.Invoke(this, EventArgs.Empty);
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

            if (!selectedAction.CanBeTaken())
            {
                // Deselect action, if it can't be taken again
                SetSelectedAction(null);
            }
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
                    SetSelectedUnit(firstUnit, true);
                }
            }
            else
            {
                SetSelectedUnit(previousSelectedUnit, true);
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
        SetSelectedUnit(null, false);
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }
}