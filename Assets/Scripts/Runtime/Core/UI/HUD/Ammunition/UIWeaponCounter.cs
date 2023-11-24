using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SA.FPS.Runtime.UI.HUD
{
    public class UIWeaponCounter : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private TextMeshProUGUI _extraCounter;
        [SerializeField] private bool _isGrenade;

        public void SetCount(int count, int extraCount, bool isShowAmmoCounter)
        {
            _counter.gameObject.SetActive(isShowAmmoCounter);
            _extraCounter.gameObject.SetActive(isShowAmmoCounter);

            if (!isShowAmmoCounter) return;

            _counter.text = (_isGrenade) ? $"{count} x" : $"x {count}";

            _extraCounter.gameObject.SetActive(extraCount > 0);
            _extraCounter.text = $"[{extraCount}]";
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}
