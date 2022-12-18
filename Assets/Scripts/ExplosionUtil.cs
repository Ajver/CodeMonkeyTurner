using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionUtil : MonoBehaviour
{

    public static ExplosionUtil Instance { get; private set; }

    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask obstaclesLayerMast;
    [SerializeField] private string wallTag;

    private void Awake()
    {
        Instance = this;
    }
    
    public List<IDamageable> GetDamagableTargetsWithinRange(Vector3 origin, float range)
    {
        List<IDamageable> damageableTargets = new List<IDamageable>();
        
        Vector3 originPosition = origin + offset;
        Collider[] colliders = Physics.OverlapSphere(originPosition, range);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Transform targetTransform = damageable.GetTransform();
                Vector3 targetPosition = targetTransform.position;
                Vector3 direction = (targetPosition - originPosition).normalized;
                RaycastHit[] hits = Physics.RaycastAll(originPosition, direction, range, obstaclesLayerMast);

                bool behindWall = false;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag(wallTag))
                    {
                        // The target is behind wall - then it's safe - let's NOT damage it 
                        behindWall = true;
                        break;
                    }
                }

                if (!behindWall)
                {
                    damageableTargets.Add(damageable);
                }
            }
        }

        return damageableTargets;
    }
    
}
