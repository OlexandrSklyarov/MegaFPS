using System;

namespace Runtime.Services.Inventory
{
    public interface IInventory
    {
        int Capacity {get;}
        bool IsFull {get;}

        event Action<object, IInventoryItem, int> InventoryItemAddedEvent;
        event Action<object, Type, int> InventoryItemRemovedEvent;
        event Action<object> InventoryStateChangedEvent;

        IInventoryItem GetItem(Type itemType);
        IInventoryItem[] GetAllItems();
        IInventoryItem[] GetAllItems(Type itemType);
        IInventoryItem[] GetEquippedItems();
        int GetItemAmount(Type itemType);

        bool TryAdd(object sender, IInventoryItem item);
        void Remove(object sender, Type itemType, int amount = 1);
        bool HasItem(Type type, out IInventoryItem item);
    }
}