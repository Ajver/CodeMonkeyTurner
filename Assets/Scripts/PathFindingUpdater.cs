using System;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate crate = sender as DestructibleCrate;
        PathFinding.Instance.SetIsWalkableGridPosition(crate.GetGridPosition(), true);
    }
}