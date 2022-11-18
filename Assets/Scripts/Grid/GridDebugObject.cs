using System;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro text;
    
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    public void Update()
    {
        text.text = gridObject.ToString();
    }
}
