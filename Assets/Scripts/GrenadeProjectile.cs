using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform explosionEffectPrefab;
    [SerializeField] private Transform trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    
    private Vector3 targetPositionXZ;
    private float totalDistance;
    private Vector3 positionXZ;
    
    private Action onGrenadeBehaviorComplete;
    
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        targetPositionXZ = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        
        positionXZ = transform.position;
        positionXZ.y = 0;
        targetPositionXZ.y = 0; 
        totalDistance = Vector3.Distance(transform.position, targetPositionXZ);
    }

    private void Update()
    {
        Vector3 moveDir = (targetPositionXZ - positionXZ).normalized;
        
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPositionXZ);
        float distanceNormalized = 0f;
        if (totalDistance == 0f)
        {
            distanceNormalized = 0f;
        }
        else
        {
            distanceNormalized = 1 - (distance / totalDistance);
        }

        float maxHeight = totalDistance * 0.2f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPositionXZ) < reachedTargetDistance)
        {
            Kaboom();
        }
    }

    private void Kaboom()
    {
        float radius = 3f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(50);
            }
        }

        trailRenderer.parent = null;
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        
        onGrenadeBehaviorComplete();
        OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
        
        Destroy(gameObject);
    }
}
