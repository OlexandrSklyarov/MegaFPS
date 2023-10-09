using System;

namespace Runtime.Services.Inventory
{
    public interface IInventoryItemInfo
    {
        string Id {get;}
        string Title {get;}
        string Description {get;}
        int MaxItemsInInventorySlot {get;}
        UnityEngine.Sprite Icon {get;}
    }
}