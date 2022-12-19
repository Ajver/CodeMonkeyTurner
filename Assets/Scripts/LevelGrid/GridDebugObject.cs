using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro text;

    protected GridPosition gridPosition;
    private object gridObject;

    public void SetGridPosition(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        text.text = gridObject.ToString();
    }
}
