using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Transform originTransform;
    [SerializeField] private LayerMask obstaclesLayerMast;
    [SerializeField] private string wallTag;
    
    public void Setup(int damage, float radius)
    {
        Explode(damage, radius);
    }
    
    private void Explode(int damage, float radius)
    {
        Vector3 originPosition = originTransform.position;
        
        Collider[] colliders = Physics.OverlapSphere(originPosition, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Transform targetTransform = damageable.GetTransform();
                Vector3 targetPosition = targetTransform.position;
                Vector3 direction = (targetPosition - originPosition).normalized;
                RaycastHit[] hits = Physics.RaycastAll(originPosition, direction, radius, obstaclesLayerMast);

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
                    damageable.Damage(damage);
                }
            }
        }
        
        // Destroy itself after a delay
        float destroyDelay = 1f;
        Destroy(gameObject, destroyDelay);
    }
    
}
