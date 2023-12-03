using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(Animator))]
    public class WeaponView : MonoBehaviour, IWeaponAttack
    {        
        [field: SerializeField] public Transform FirePoint {get; private set;}
        [field: SerializeField] public WeaponSettings Settings {get; private set;}
        
        [SerializeField] private float _reloadTime = 1f;
        [SerializeField] private float _attackTime = 0.5f;

        DamageWeaponSettings IWeaponAttack.DamageSettings => Settings;

        private Animator _weaponAnimator;
        

        private void Awake() 
        {
            _weaponAnimator = GetComponent<Animator>();    
        }


        public bool TryReload(out float reloadTime)
        {
            reloadTime = 0;

            if (!Settings.IsRangeWeapon) 
            {
                return false;
            }

            reloadTime = _reloadTime;
            _weaponAnimator.SetTrigger("RELOAD");
            
            return true;
        }


        public void MeleeAttack(out float reloadTime)
        {
            reloadTime = _attackTime;
            _weaponAnimator.SetTrigger("MELEE_ATTACK");
        }
    }
}