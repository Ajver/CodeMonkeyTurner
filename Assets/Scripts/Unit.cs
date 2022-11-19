using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator;

    private MoveAction moveAction;

    private GridPosition gridPosition;

    private void Start()
    {
        moveAction = GetComponent<MoveAction>();
        
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    
    private void Update()
    {   
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public Animator GetAnimator()
    {
        return unitAnimator;
    }
    
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
