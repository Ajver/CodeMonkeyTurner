using System;
using UnityEngine;

public class DestructibleCrate : GridOccupant, IDamageable
{

    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    public void Damage(int dmg)
    {
        Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    protected override void OccupantStart()
    {
    }

    protected override void OccupantUpdate()
    {
    }

    public GameTeam GetGameTeam()
    {
        return GameTeam.Neutral;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
