using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace SA.FPS.Runtime.UI.HUD
{
    [RequireComponent(typeof(Image))]
    public class UIWeaponView : MonoBehaviour, IPoolable<UIWeaponView>
    {
        private Image _icon;
        private IObjectPool<UIWeaponView> _myPool;

        private void Awake() 
        {
            _icon = GetComponent<Image>();    
        }

        public void SetIcon(Sprite icon, bool isFocus)
        {
            _icon.sprite = icon;

            var color = _icon.color;
            color.a = (isFocus) ? 1f : 0.5f;
            _icon.color = color;
        }

        public void Hide() => _myPool.Release(this);


        void IPoolable<UIWeaponView>.SetPool(IObjectPool<UIWeaponView> pool)
        {
            _myPool = pool;
        }
    }
}