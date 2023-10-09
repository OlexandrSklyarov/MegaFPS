using System;

namespace Runtime.Services.Inventory
{
    [Serializable]
    public class InventoryItemState : IInventoryItemState
    {
        public bool IsEquipped { get => IsItemEquipped; set => IsItemEquipped = value; }
        public int Amount { get => ItemAmount; set => ItemAmount = value; }
    

        public int ItemAmount;
        public bool IsItemEquipped;
    }
}