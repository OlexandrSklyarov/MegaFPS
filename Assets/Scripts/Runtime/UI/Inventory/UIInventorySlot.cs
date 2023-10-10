using System;
using Runtime.Services.Inventory;
using UnityEngine.EventSystems;

namespace Runtime.UI.Inventory
{
    public class UIInventorySlot : UISlot
    {
        public IInventorySlot Slot {get; private set;}

        private UIInventory _uiInventory;
        private UIInventoryItem _uiInventoryItem;

        public void Init(UIInventory uiInventory)
        {
            _uiInventory = uiInventory;
            _uiInventoryItem = GetComponentInChildren<UIInventoryItem>();
        }


        public void SetSlot(IInventorySlot newSlot)
        {
            Slot = newSlot;
        }


        public override void OnDrop(PointerEventData eventData)
        {
            var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
            var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
            var otherSlot = otherSlotUI.Slot; 

            _uiInventory.Inventory.TransitFromSlotToSlot(this, otherSlot, Slot);

            Refresh();
            otherSlotUI.Refresh();
        }


        public void Refresh()
        {
            if (Slot != null) _uiInventoryItem.Refresh(Slot);
        }
    }
}
