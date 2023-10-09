
namespace Runtime.Services.Inventory
{
    public interface IInventorySlot
    {
        bool IsFull {get;}
        bool IsEmpty {get;}

        IInventoryItem Item {get;}
        System.Type ItemType {get;}
        int Amount {get;}
        int Capacity {get;}

        void SetItem(IInventoryItem item);
        void Clear();
    }
}