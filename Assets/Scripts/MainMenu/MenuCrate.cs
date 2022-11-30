using UnityEngine;

public class MenuCrate : MonoBehaviour, IDamageable
{
    
    [SerializeField] private Transform crateDestroyedPrefab;

    public void Damage(int dmg)
    {
        Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public GameTeam GetGameTeam()
    {
        return GameTeam.Neutral;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
