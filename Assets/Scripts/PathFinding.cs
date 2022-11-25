using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public static PathFinding Instance { get; private set; }
    
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    
    private const int MOVE_STAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one PathFinding in the scene!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (_gs, position) => new PathNode(position));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                        raycastOffsetDistance * 2f, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    private int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPosDistance = a - b;
        int xDist = Mathf.Abs(gridPosDistance.x);
        int zDist = Mathf.Abs(gridPosDistance.z);
        int diagonalCost = Mathf.Min(xDist, zDist) * MOVE_DIAGONAL_COST;
        int remaining = Mathf.Abs(xDist - zDist) * MOVE_STAIGHT_COST;
        return diagonalCost + remaining;
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode node = gridSystem.GetGridObject(gridPosition);

                node.SetGCost(int.MaxValue);
                node.SetHCost(0);
                node.CalculateFCost();
                node.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();

        PathNode endNode = gridSystem.GetGridObject(endPosition);
        
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int distance = CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                int tentativeGCost = currentNode.GetGCost() + distance;

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        
        // No path found
        return null;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        foreach (PathNode node in pathNodeList)
        {
            if (node.GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = node;
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x > 0)
        {
            // Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));

            if (gridPosition.z < gridSystem.GetHeight() - 1)
            {
                // Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if (gridPosition.z > 0)
            {
                // Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.x < gridSystem.GetWidth() - 1)
        {
            // Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));

            if (gridPosition.z < gridSystem.GetHeight() - 1)
            {
                // Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }

            if (gridPosition.z > 0)
            {
                // Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }

        if (gridPosition.z < gridSystem.GetHeight() - 1)
        {
            // Up
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1));
        }

        if (gridPosition.z > 0)
        {
            // Down
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1));
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>();
        pathNodes.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodes.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }
        
        pathNodes.Reverse();

        List<GridPosition> gridPositions = new List<GridPosition>();

        foreach (PathNode node in pathNodes)
        {
            gridPositions.Add(node.GetGridPosition());
        }

        return gridPositions;
    }

}
