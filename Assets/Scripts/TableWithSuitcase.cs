using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWithSuitcase : GridOccupant, IInteractable, IDamageable
{
    protected override void OccupantStart()
    {
    }

    protected override void OccupantUpdate()
    {
    }

    public void Damage(int dmg)
    {
        BreakIntoParts();
    }

    private void BreakIntoParts()
    {
        // TODO
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
        // TODO: Hide the suitcase
        // TODO: Win the game
    }
}
