using System;
using UnityEngine;

public class DestructibleCrate : GridOccupant, IDamageable
{

    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    public void Damage(int dmg)
    {
        Transform destroyedCrateObject = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(destroyedCrateObject, 150f, transform.position, 10f);
            
        Destroy(gameObject);
        ClearItselfFromGrid();
        
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
    
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
