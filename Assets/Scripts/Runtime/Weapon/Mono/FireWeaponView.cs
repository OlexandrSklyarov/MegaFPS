using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    public class FireWeaponView : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePoint {get; private set;}
        [field: SerializeField] public WeaponSettings Settings {get; private set;}        
        [field: SerializeField] public Animator WeaponAnimator {get; private set;}
        [field: SerializeField] public WeaponType Type {get; private set;}

        private void Start()  => Hide();

        public void Show() => WeaponAnimator.gameObject.SetActive(true);

        public void Hide() => WeaponAnimator.gameObject.SetActive(false);
    }
}