using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeEffectPrefab;
    [SerializeField] private Transform trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    
    private Vector3 targetPosition;
    private float totalDistance;
    private Vector3 positionXZ;
    
    private Action onGrenadeBehaviorComplete;
    
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(transform.position, targetPosition);

    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - (distance / totalDistance);

        float maxHeight = totalDistance * 0.2f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            Kaboom();
        }
    }

    private void Kaboom()
    {
        float radius = 4f;
        Collider[] colliders = Physics.OverlapSphere(targetPosition, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Unit unit))
            {
                unit.Damage(30);
            }
            
            if (collider.TryGetComponent(out DestructibleCrate crate))
            {
                crate.Damage();
            }
        }

        trailRenderer.parent = null;
        Instantiate(grenadeExplodeEffectPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
        
        onGrenadeBehaviorComplete();
        OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
        
        Destroy(gameObject);
    }
}
