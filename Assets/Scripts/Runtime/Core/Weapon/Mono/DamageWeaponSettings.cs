using NaughtyAttributes;
using UnityEngine;

namespace SA.FPS
{
    public abstract class DamageWeaponSettings : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int Damage {get; private set;} = 15;      
        [field: Space, SerializeField] public bool IsUsedPushForce {get; private set;}     
        [field: ShowIf("IsUsedPushForce"), SerializeField, Min(1)] public int PushPower {get; private set;} = 100;        
    }
}