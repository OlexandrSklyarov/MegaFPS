using UnityEngine;
using UnityEngine.UI;

namespace SA.FPS.Runtime.UI.HUD
{
    [RequireComponent(typeof(Image))]
    public class UIWeaponView : MonoBehaviour
    {
        private Image _icon;

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
    }
}