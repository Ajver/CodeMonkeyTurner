using UnityEngine;

public interface IShootable
{

    void Damage(int dmg);

    GameTeam GetGameTeam();

    Transform GetTransform();

}
