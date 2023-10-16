using DG.Tweening;
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

        [SerializeField] private Transform _hideShowRoot;

        private void Start()  => Hide();

        public void Show() 
        {
            WeaponAnimator.gameObject.SetActive(true);

            _hideShowRoot
                .DOLocalRotateQuaternion(Quaternion.identity, 0.5f)
                .SetEase(Ease.InOutCubic);
        }

        public void Hide()
        {
            _hideShowRoot
                .DOLocalRotateQuaternion(Quaternion.Euler(45f, 0f, 0f), 0.5f)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => WeaponAnimator.gameObject.SetActive(false));            
        }
    }
}