using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SA.FPS.Runtime.UI.HUD
{
    public class UIWeaponCounter : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private bool _isGrenade;

        public void SetCount(int count)
        {
            _counter.text = (_isGrenade) ? $"{count} x" : $"{count}";
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}
