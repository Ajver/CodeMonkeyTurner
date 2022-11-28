using System;
using UnityEngine;

public class ExplodingBarrel : GridOccupant, IInteractable, IDamageable
{
    [SerializeField] private Transform explosionEffectPrefab;

    public static event EventHandler OnAnyBarrelExploded;
    
    private bool firedToExplode;
    private Action onInteractComplete;

    protected override void OccupantStart()
    {
    }

    protected override void OccupantUpdate()
    {
        if (!firedToExplode)
        {
            return;
        }

        firedToExplode = false;
        Explode();

        if (onInteractComplete != null)
        {
            // This may be null when fired by projectile, instead of interaction
            onInteractComplete();
        }
    }

    public void Damage(int dmg)
    {
        FireToExplode();
    }

    public GameTeam GetGameTeam()
    {
        return GameTeam.Neutral;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        FireToExplode();
    }

    private void FireToExplode()
    {
        // Prepares to fire - will explode in the next frame
        firedToExplode = true;
    }
    
    private void Explode()
    {
        float radius = 5f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(150);
            }
        }
        
        Destroy(gameObject);

        Vector3 offset = Vector3.up * 0.5f;
        Instantiate(explosionEffectPrefab, transform.position + offset, transform.rotation);

        OnAnyBarrelExploded?.Invoke(this, EventArgs.Empty);
        
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
 
}
