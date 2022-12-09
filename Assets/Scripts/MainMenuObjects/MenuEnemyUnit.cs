using System;
using UnityEngine;

public class MenuEnemyUnit : MonoBehaviour, IDamageable
{
    public static event EventHandler OnAnyUnitDamaged;
    
    private HealthSystem healthSystem;
    
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    public void Damage(int dmg)
    {
        healthSystem.Damage(dmg);
        OnAnyUnitDamaged?.Invoke(this, EventArgs.Empty);
    }

    public GameTeam GetGameTeam()
    {
        return GameTeam.Enemy;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
