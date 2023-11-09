using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "SO/WeaponSettings")]
    public class WeaponSettings : DamageWeaponSettings
    {            
        [field: Header("Shake"), SerializeField] public float Strength {get; private set;} = 1f;
        [field: SerializeField] public float Randomness {get; private set;} = 90f;
        [field: SerializeField] public int Vibrato {get; private set;} = 10;
        public float ShakeDuration => AttackCooldown;
        [field: Space, SerializeField, Min(0.01f)] public float AttackCooldown {get; private set;} = 0.5f;
        [field: SerializeField] public LayerMask TargetLayerMask {get; private set;} 

        [field: Space, Header("Icon"), Required, SerializeField] public Sprite Sprite {get; private set;}
        [field: Space, Header("Audio"), SerializeField] public EventReference AttackSfx {get; private set;}

        public bool IsRangeWeapon => _isRangeWeapon;
        public int StartAmmo => _startAmmo;
        public int MagAmountAmmo => _magAmountAmmo;
        public int RayCountPerShoot => _rayCountPerShoot;
        public float Distance => _distance;
        public DecalType DecalType => _decalType;
        public bool IsUseSpread => _isUseSpread;
        public float SpreadFactor => _spreadFactor;

        [Space, SerializeField] private bool _isRangeWeapon;
        [Header("Shoot"), SerializeField, ShowIf("IsRangeWeapon"), Min(1)] private int _startAmmo = 60;
        [SerializeField, ShowIf("IsRangeWeapon"), Min(1)] private int _magAmountAmmo = 60;
        [SerializeField, ShowIf("IsRangeWeapon"), Min(1)] private int _rayCountPerShoot = 1;
        [SerializeField, ShowIf("IsRangeWeapon"), Min(1f)] private float _distance = Mathf.Infinity;
        [SerializeField,ShowIf("IsRangeWeapon")] private DecalType _decalType;
        [SerializeField, ShowIf("IsRangeWeapon")] private bool _isUseSpread;
        [SerializeField, ShowIf("IsRangeWeapon"), Min(0f)] private float _spreadFactor;      
    }
}