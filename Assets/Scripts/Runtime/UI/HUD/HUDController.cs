using System;
using Runtime.Services.Inventory;
using SA.FPS.Runtime.UI.HUD;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIWeaponCounter _weaponCounter;
        [SerializeField] private UIWeaponCounter _grenadeCounter;

        private IInventory _inventory;

        public void Init(IInventory inventory)
        {
            _inventory = inventory;            
            _inventory.InventoryItemAddedEvent += OnItemAdd;
            _inventory.InventoryStateChangedEvent += OnChangeState;
            _inventory.InventoryItemRemovedEvent += OnItemRemove;
        }

        private void OnItemRemove(object arg1, Type type, int count)
        {
            DebugUtility.PrintColor($"OnItemRemove {type} {count}", Color.cyan);
        }

        private void OnChangeState(object obj)
        {
            DebugUtility.PrintColor($"OnChangeState", Color.yellow);
            Array.ForEach(_inventory.GetAllItems(), x =>
            {
                DebugUtility.PrintColor($"Item {x.Info.Title}: {x.State.Amount}", Color.cyan);
            });            
        }

        private void OnItemAdd(object arg1, IInventoryItem item, int count)
        {
            DebugUtility.PrintColor($"OnItemRemove {item.Info.Title} {count}", Color.cyan);
        }

        //weapon count
        public void UpdateWeaponPanel(int count)
        {
            _weaponCounter.SetCount(count);
            DebugUtility.PrintColor($"UpdateWeaponPanel count{count}", Color.yellow);
        }

        //change weapon
        public void UpdateWeaponPanel(int count, Sprite icon)
        {
            UpdateWeaponPanel(count);
            _weaponCounter.SetIcon(icon);
        }
    }
}