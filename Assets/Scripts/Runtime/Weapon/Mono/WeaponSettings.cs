using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "SO/WeaponSettings")]
    public class WeaponSettings : ScriptableObject
    {        
        [field: Header("Shake"), SerializeField] public float Strength {get; private set;} = 1f;
        [field: SerializeField] public float Randomness {get; private set;} = 90f;
        [field: SerializeField] public int Vibrato {get; private set;} = 10;
        public float ShakeDuration => ShootCooldown;

     
        [field: Space, Header("Shoot"), SerializeField, Min(0.01f)] public float ShootCooldown {get; private set;} = 0.5f;
        [field: SerializeField, Min(1)] public int StartAmmo {get; private set;} = 60;
        [field: SerializeField, Min(1)] public int Damage {get; private set;} = 15;
        [field: SerializeField, Min(1)] public int RayCountPerShoot {get; private set;} = 1;
        [field: SerializeField, Min(1f)] public float Distance {get; private set;} = Mathf.Infinity;
        [field: SerializeField] public DecalType DecalType {get; private set;}
        [field: SerializeField] public bool IsUseSpread {get; private set;}
        [field: EnableIf("IsUseSpread"), SerializeField, Min(0f)] public float SpreadFactor {get; private set;}
        [field: SerializeField] public LayerMask TargetLayerMask {get; private set;} 

     
        [field: Space, Header("Audio"), SerializeField] public EventReference FireSfx{get; private set;}
    }
}