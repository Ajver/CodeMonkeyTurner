using System;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour, IInteractable, IDamageable
{
    private GridPosition gridPosition;

    private bool firedToExplode;
    private Action onInteractComplete;
    
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        LevelGrid.Instance.SetDamageableAtGridPosition(gridPosition, this);
    }

    private void Update()
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
        Debug.Log(gameObject + " Exploded!");
        Destroy(gameObject);
        
        LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);
        LevelGrid.Instance.ClearDamageableAtGridPosition(gridPosition);
    }
    
}
