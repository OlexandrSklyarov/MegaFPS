using System;

namespace Runtime.Services.Inventory
{
    public interface IInventoryItemState
    {
        bool IsEquipped {get; set;}
        int Amount {get; set;}
    }
}