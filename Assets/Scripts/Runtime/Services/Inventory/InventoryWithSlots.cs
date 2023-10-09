using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Runtime.Services.Inventory
{
    public class InventoryWithSlots : IInventory
    {
        public int Capacity {get; private set;}
        public bool IsFull => _slots.All(s => s.IsFull);

        private List<IInventorySlot> _slots;

        public event Action<object, IInventoryItem, int> InventoryItemAddedEvent;
        public event Action<object, Type, int> InventoryItemRemovedEvent;
        public event Action<object> InventoryStateChangedEvent;

        public InventoryWithSlots(int capacity)
        {
            Capacity = capacity;
            
            _slots = new List<IInventorySlot>(Capacity);
            for (int i = 0; i < Capacity; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }

        public IInventoryItem[] GetAllItems()
        {
            var items = new List<IInventoryItem>();
            
            foreach(var slot in _slots)
            {
                if (!slot.IsEmpty)
                    items.Add(slot.Item);
            }

            return items.ToArray();
        }


        public IInventoryItem[] GetAllItems(Type itemType)
        {
            var items = new List<IInventoryItem>();
            var slotsOfType = _slots.FindAll(s => !s.IsEmpty && s.ItemType == itemType);
            
            foreach(var slot in slotsOfType)
            {
                if (!slot.IsEmpty)
                    items.Add(slot.Item);
            }

            return items.ToArray();
        }


        public IInventoryItem[] GetEquippedItems()
        {
            var equippedItems = new List<IInventoryItem>();
            var requiredSlots = _slots.FindAll(s => !s.IsEmpty &&s.Item.State.IsEquipped);

            foreach(var slot in requiredSlots)
            {
                equippedItems.Add(slot.Item);
            }

            return equippedItems.ToArray();
        }


        public IInventoryItem GetItem(Type itemType)
        {
            return _slots.Find(s => s.ItemType == itemType).Item;
        }


        public int GetItemAmount(Type itemType)
        {
            var amount = 0;
            var allItemSlots = _slots.FindAll(s => !s.IsEmpty && s.ItemType == itemType);

            foreach(var slot in allItemSlots)
            {
                amount += slot.Amount;
            }

            return amount;
        }


        public bool TryAdd(object sender, IInventoryItem item)
        {
            var slotWithItem = _slots
                .Find(s => !s.IsEmpty && s.ItemType == item.Type && !s.IsFull);

            if (slotWithItem != null)
            {
                return TryAddToSlot(sender, slotWithItem, item);
            }

            var emptySlot = _slots.Find(s => s.IsEmpty);
            if (emptySlot != null)
            {
                return TryAddToSlot(sender, emptySlot, item);
            }

            Util.DebugUtility.PrintColor($"Cannot add item ({item.Type}), amount: {item.State.Amount}, inventory is full", UnityEngine.Color.yellow);
            return false;
        }


        private bool TryAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
        {
            var isFits = slot.Amount + item.State.Amount <= item.Info.MaxItemsInInventorySlot;

            var amountToAdd = (isFits) ?
                item.State.Amount :
                item.Info.MaxItemsInInventorySlot - slot.Amount;

            if (slot.IsEmpty)
            {
                var clone = item.Clone();
                clone.State.Amount = amountToAdd;
                slot.SetItem(clone);
            }
            else
            {
                slot.Item.State.Amount += amountToAdd;
            }

            InventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
            InventoryStateChangedEvent?.Invoke(sender);

            var amountLeft = item.State.Amount - amountToAdd;
            
            if (amountLeft <= 0)
                return true;

            item.State.Amount = amountLeft;

            return TryAdd(sender, item);            
        }


        public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
        {
            if (fromSlot.IsEmpty) return;
            if (toSlot.IsFull) return;
            if (!toSlot.IsEmpty && fromSlot.ItemType != toSlot.ItemType) return;

            var slotCapacity = fromSlot.Capacity;
            var isFits = fromSlot.Amount + toSlot.Amount <= slotCapacity;
            var amountToAdd = (isFits) ? fromSlot.Amount : slotCapacity - toSlot.Amount;
            var amountLeft = fromSlot.Amount - amountToAdd;

            if (toSlot.IsEmpty)
            {
                toSlot.SetItem(fromSlot.Item);
                fromSlot.Clear();
                InventoryStateChangedEvent?.Invoke(sender);
            }

            toSlot.Item.State.Amount += amountToAdd;
            if (isFits)
            {
                fromSlot.Clear();
            }
            else
            {
                fromSlot.Item.State.Amount = amountLeft;
            }
        }


        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotWithItem = GetAllSlots(itemType);

            if (slotWithItem.Length == 0) return;

            var amountToRemove = amount;

            for (int i = slotWithItem.Length - 1; i >= 0; i--)
            {
                var slot = slotWithItem[i];
                
                if (slot.Amount >= amountToRemove)
                {
                    slot.Item.State.Amount -= amountToRemove;    

                    if (slot.Amount <= 0) 
                    {
                        slot.Clear();
                    }

                    InventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                    InventoryStateChangedEvent?.Invoke(sender);

                    break;
                }

                var amountRemoved = slot.Amount;
                amountToRemove -= slot.Amount;

                InventoryItemRemovedEvent?.Invoke(sender, itemType, amountRemoved);
                InventoryStateChangedEvent?.Invoke(sender);
                
                slot.Clear();
            }
        }


        private IInventorySlot[] GetAllSlots(Type itemType)
        {
            return _slots.FindAll(s => !s.IsEmpty && s.ItemType == itemType).ToArray();
        }
        

        public bool HasItem(Type type, out IInventoryItem item)
        {
            item = GetItem(type);
            return item != null;
        }


        public IInventorySlot[] GetAllSlots() => _slots.ToArray();
    }
}