using System;
using UnityEngine;

public class TableWithSuitcase : GridOccupant, IInteractable, IDamageable
{

    [SerializeField] private Suitcase suitcase;

    [SerializeField] private AudioSource interactAudio;
    
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
        onInteractionComplete();
        
        OnAnyTreasureCollected?.Invoke(this, EventArgs.Empty);
    }
    
    public void Damage(int dmg)
    {
        BreakIntoParts();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void BreakIntoParts()
    {
        ClearItselfFromGrid();

        Destroy(gameObject);
        
        // TODO: visuals
        OnAnyTreasureDestroyed?.Invoke(this, EventArgs.Empty);

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

        interactAudio.Play();
        suitcase.Open();
        
        // Remove from grid, but still make the spot not-walkable
        LevelGrid.Instance.ClearOccupantAtGridPosition(gridPosition);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

}
