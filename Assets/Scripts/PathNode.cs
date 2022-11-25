using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;
    
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    
    public int GetGCost()
    {
        return gCost;
    }
    
    public int GetHCost()
    {
        return hCost;
    }
    
    public int GetFCost()
    {
        return fCost;
    }
    
    
    public void SetGCost(int cost)
    {
        gCost = cost;
    }
    
    public void SetHCost(int cost)
    {
        hCost = cost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode pn)
    {
        cameFromPathNode = pn;
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool flag)
    {
        isWalkable = flag;
    }
    
}

