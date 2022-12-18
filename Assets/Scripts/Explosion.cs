using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public void Explode(int damage, float radius)
    {
        foreach (IDamageable damageable in ExplosionUtil.Instance.GetDamagableTargetsWithinRange(transform.position, radius))
        {
            damageable.Damage(damage);
        }
        
        // Destroy itself after a delay
        float destroyDelay = 1f;
        Destroy(gameObject, destroyDelay);
    }

}
