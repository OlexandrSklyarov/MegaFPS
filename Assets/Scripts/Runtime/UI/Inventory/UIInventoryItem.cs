using Runtime.Services.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Inventory
{
    public class UIInventoryItem : UIItem
    {
        public IInventoryItem Item {get; private set;}

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _textAmount;

        public void Refresh(IInventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                Cleanup();
                return;
            }

            Item = slot.Item;
            _icon.gameObject.SetActive(true);
            _icon.sprite = Item.Info.Icon;

            var isActiveTextAmount = slot.Amount > 1;
            _textAmount.gameObject.SetActive(isActiveTextAmount);

            if (isActiveTextAmount)
            {
                _textAmount.text = $"x{slot.Amount}";
            }
        }

        private void Cleanup()
        {
            _icon.gameObject.SetActive(false);
            _textAmount.gameObject.SetActive(false);
        }
    }
}
