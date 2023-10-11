using System;
using Runtime.Services.Inventory;

namespace Runtime.Services.WeaponsFactory.Items
{
    public class MachineGun : IInventoryItem
    {
        public IInventoryItemInfo Info {get;}
        public IInventoryItemState State {get; set;}
        public Type Type {get;}
        

        public MachineGun(IInventoryItemInfo info)
        {
            Info = info;
            State = new InventoryItemState();
        }


        public IInventoryItem Clone()
        {
            var clone = new MachineGun(Info);
            clone.State.Amount = State.Amount;
            return clone;
        }
    }
}