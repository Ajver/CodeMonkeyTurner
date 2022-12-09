using System;
using UnityEngine;

public class UnitAudioPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource hitAudio;
    
    private void Start()
    {
        Unit.OnAnyUnitDamaged += Unit_OnAnyUnitDamaged;
        MenuEnemyUnit.OnAnyUnitDamaged += MenuEnemyUnit_OnAnyUnitDamaged;
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDamaged -= Unit_OnAnyUnitDamaged;
        MenuEnemyUnit.OnAnyUnitDamaged -= MenuEnemyUnit_OnAnyUnitDamaged;
    }

    private void Unit_OnAnyUnitDamaged(object sender, EventArgs e)
    {
        hitAudio.Play();
    }

    private void MenuEnemyUnit_OnAnyUnitDamaged(object sender, EventArgs e)
    {
        hitAudio.Play();
    }
}
