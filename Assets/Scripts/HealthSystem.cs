using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthSystem : MonoBehaviour
{
    
    [SerializeField] private int health = 100;

    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;

    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
        
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        
        Debug.Log(health);
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
    
}
