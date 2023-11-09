using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(Animator))]
    public class WeaponView : MonoBehaviour, IWeaponAttack
    {        
        [field: SerializeField] public Transform FirePoint {get; private set;}
        [field: SerializeField] public WeaponSettings Settings {get; private set;}

        DamageWeaponSettings IWeaponAttack.DamageSettings => Settings;

        private Animator _weaponAnimator;

        private void Awake() 
        {
            _weaponAnimator = GetComponent<Animator>();    
        }


        public bool TryReload(out float reloadTime)
        {
            reloadTime = 0;

            if (!Settings.IsRangeWeapon) return false;

            _weaponAnimator.SetTrigger("RELOAD");
            reloadTime = 1f;
            
            return true;
        }


        public void MeleeAttack(out float reloadTime)
        {
            _weaponAnimator.SetTrigger("MELEE_ATTACK");
            reloadTime = 1f;
        }
    }
}