using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    
    private void Update()
    {
        bool hasSelected = TryHandleUnitSelection();

        if (!hasSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedUnit.Move(MouseWorld.GetPosition());
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask);

            if (hit)
            {
                if (hitInfo.collider.TryGetComponent<Unit>(out Unit unit))
                {
                    Debug.Log("Selected: " + selectedUnit.name);
                    selectedUnit = unit;

                    return true;
                }
            }
            
        }
        
        return false;
    }
}