using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator;

    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionsArray;

    private GridPosition gridPosition;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionsArray = GetComponents<BaseAction>();
    }

    private void Start()
    {   
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
    
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActionsArray()
    {
        Debug.Log("All actions....?");
        Debug.Log(baseActionsArray);
        return baseActionsArray;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
