using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
    }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    
    public static GridSystemVisual Instance { get; private set; }
    
    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one GridSystemVisual in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        
        gridSystemVisualSingles = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(), 
            LevelGrid.Instance.GetHeight()
        ];
        
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform visualSingle = Instantiate(
                    gridSystemVisualSinglePrefab, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), 
                    Quaternion.identity
                );
                gridSystemVisualSingles[x, z] = visualSingle.GetComponent<GridSystemVisualSingle>();
            }
        }

        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        foreach (GridSystemVisualSingle visualSingle in gridSystemVisualSingles)
        {
            visualSingle.Hide();
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                gridPositionList.Add(testGridPosition);
            }
        }
        
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                gridPositionList.Add(testGridPosition);
            }
        }
        
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        Material material = GetGridVisualMaterial(gridVisualType);
        
        foreach (GridPosition gridPos in gridPositions)
        {
            gridSystemVisualSingles[gridPos.x, gridPos.z].Show(material);
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        if (selectedAction == null)
        {
            // Maybe the unit has been deselected (i.e. killed)
            return;
        }
        
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        List<GridPosition> validPositions = selectedAction.GetValidActionGridPositionList();

        GridVisualType gridVisualType = GridVisualType.White;

        switch (selectedAction)
        {
            case MoveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case GrenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        
        ShowGridPositionList(validPositions, gridVisualType);
    }

    private Material GetGridVisualMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        
        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType: " + gridVisualType);
        return null;
    }
    
}
