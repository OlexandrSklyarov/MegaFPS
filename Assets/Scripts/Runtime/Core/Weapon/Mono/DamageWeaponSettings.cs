using UnityEngine;

namespace SA.FPS
{
    public abstract class DamageWeaponSettings : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int Damage {get; private set;} = 15;
        [field: SerializeField, Min(1)] public int HeadShotDamageMultiplier {get; private set;} = 3;        
    }
}