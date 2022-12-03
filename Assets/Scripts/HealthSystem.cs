using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthSystem : MonoBehaviour
{
    
    [SerializeField] private int health = 100;

    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;

    private int healthMax;

    private bool isAlive = true;
    
    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        if (!isAlive)
        {
            return;
        }

        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
        
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Die()
    {
        isAlive = false;
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
    
}
