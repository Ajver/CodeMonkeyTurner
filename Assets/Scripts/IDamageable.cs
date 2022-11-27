using UnityEngine;

public interface IDamageable
{

    void Damage(int dmg);

    GameTeam GetGameTeam();

    Transform GetTransform();

}
