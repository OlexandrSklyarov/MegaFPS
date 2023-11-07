using UnityEngine;

namespace SA.FPS
{
    /// <summary>
    /// weapon model teg
    /// </summary>
    public class WeaponView : MonoBehaviour, IWeaponView
    {        
        [field: SerializeField] public Transform FirePoint {get; private set;}
        [field: SerializeField]public WeaponSettings Settings {get; private set;}
        [field: SerializeField] private Animator WeaponAnimator;


        public bool TryReload(out float reloadTime)
        {
            reloadTime = 0;

            if (Settings.IsRangeWeapon) return false;

            WeaponAnimator.SetTrigger("RELOAD");
            reloadTime = 1f;
            
            return true;
        }


        public void MeleeAttack(out float reloadTime)
        {
            WeaponAnimator.SetTrigger("MELEE_ATTACK");
            reloadTime = 1f;
        }
    }
}