using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    [SerializeField] private Transform gridSystemVisualSinglePrefab;

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

    public void ShowGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPos in gridPositions)
        {
            gridSystemVisualSingles[gridPos.x, gridPos.z].Show();
        }
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        List<GridPosition> validPositions = selectedAction.GetValidActionGridPositionList();
        ShowGridPositionList(validPositions);
    }
}
