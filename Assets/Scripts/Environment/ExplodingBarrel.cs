using System;
using UnityEngine;

public class ExplodingBarrel : GridOccupant, IInteractable, IDamageable
{
    [SerializeField] private Explosion explosionPrefab;
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
        int damage = 150;
        float radius = 5f;

        Explosion explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.Explode(damage, radius);
        
        Destroy(gameObject);
        ClearItselfFromGrid();
        
        OnAnyBarrelExploded?.Invoke(this, EventArgs.Empty);
    }
 
}
