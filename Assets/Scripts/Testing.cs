using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositions = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i < gridPositions.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositions[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositions[i + 1]),
                    Color.white,
                    10f
                );
            }
        }
    }
}