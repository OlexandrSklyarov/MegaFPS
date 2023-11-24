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
            if (!Settings.IsRangeWeapon) 
            {
                reloadTime = 0;
                return false;
            }

            reloadTime = 1f;
            _weaponAnimator.SetTrigger("RELOAD");
            
            return true;
        }


        public void MeleeAttack(out float reloadTime)
        {
            reloadTime = 1f;
            _weaponAnimator.SetTrigger("MELEE_ATTACK");
        }
    }
}