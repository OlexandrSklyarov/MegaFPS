
namespace Runtime.Services.Inventory
{
    public interface IInventoryItem
    {
        IInventoryItemInfo Info {get;}
        IInventoryItemState State {get; set;}
        System.Type Type {get;}

        IInventoryItem Clone();
    }
}