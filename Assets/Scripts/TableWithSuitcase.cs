using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWithSuitcase : GridOccupant, IInteractable, IDamageable
{

    [SerializeField] private GameObject treasureGameObject;

    public static event EventHandler OnAnyTreasureCollected;
    public static event EventHandler OnAnyTreasureDestroyed;
    
    private bool isActive;
    private float timer;
    private Action onInteractionComplete;
    
    protected override void OccupantStart()
    {
    }

    protected override void OccupantUpdate()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            CollectTreasure();
            isActive = false;
        }
    }

    private void CollectTreasure()
    {
        treasureGameObject.SetActive(false);
        onInteractionComplete();
        
        OnAnyTreasureCollected?.Invoke(this, EventArgs.Empty);
        Debug.Log("MISSION COMPLETE!");
    }
    
    public void Damage(int dmg)
    {
        BreakIntoParts();
    }

    private void BreakIntoParts()
    {
        // TODO: visuals
        OnAnyTreasureDestroyed?.Invoke(this, EventArgs.Empty);
        
        Destroy(gameObject);
    }
    
    public GameTeam GetGameTeam()
    {
        // Player team, so it cannot be hit by player shoot or sword
        return GameTeam.Player;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Action onInteractComplete)
    {
        onInteractionComplete = onInteractComplete;
        isActive = true;
        timer = 0.7f;
        
        // Remove from grid, but still make the spot not-walkable
        LevelGrid.Instance.ClearOccupantAtGridPosition(gridPosition);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
