using System;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    [SerializeField] private Transform gridDebugObjectPrefab;
    
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (_gs, position) => new PathNode(position));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
}
