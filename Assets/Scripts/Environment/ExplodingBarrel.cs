using System;
using UnityEngine;

public class ExplodingBarrel : GridOccupant, IInteractable, IDamageable
{
    [SerializeField] private Transform explosionEffectPrefab;
    [SerializeField] private AudioSource hitAudio;

    public static event EventHandler OnAnyBarrelExploded;

    private enum State
    {
        Kalm,
        FiredToExplode,
        Exploding,
    }

    private State state = State.Kalm;
    private float stateTimer;

    private Action onInteractComplete;

    protected override void OccupantStart()
    {
    }

    protected override void OccupantUpdate()
    {
        if (state != State.FiredToExplode)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        if (state == State.FiredToExplode)
        {
            Explode();
            
            if (onInteractComplete != null)
            {
                // This may be null when fired by projectile, instead of interaction
                onInteractComplete();
            }

            state = State.Exploding;
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
        state = State.FiredToExplode;
        float firingTimer = 0.1f;
        stateTimer = firingTimer;
        
        hitAudio.Play();
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
        ClearItselfFromGrid();
        
        Vector3 offset = Vector3.up * 0.5f;
        Instantiate(explosionEffectPrefab, transform.position + offset, transform.rotation);

        OnAnyBarrelExploded?.Invoke(this, EventArgs.Empty);
    }
 
}
