using System;
using UnityEngine;

public class UnitAudioPlayer : MonoBehaviour
{

    [SerializeField] private AudioSource hitAudio;
    
    private void Start()
    {
        Unit.OnAnyUnitDamaged += Unit_OnAnyUnitDamaged;
    }

    private void OnDestroy()
    {
        Unit.OnAnyUnitDamaged -= Unit_OnAnyUnitDamaged;
    }

    private void Unit_OnAnyUnitDamaged(object sender, EventArgs e)
    {
        hitAudio.Play();
    }
}
