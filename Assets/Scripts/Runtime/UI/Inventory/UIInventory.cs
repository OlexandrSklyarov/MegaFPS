using Runtime.Services.Inventory;
using System;
using UnityEngine;

namespace Runtime.UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        public InventoryWithSlots Inventory {get; private set;}

        [SerializeField] private Transform _content;


        private void Awake() 
        {
            Inventory = new InventoryWithSlots(_content.childCount);

            Array.ForEach(GetComponentsInChildren<UIInventorySlot>(true), x =>
            {
                x.Init(this);
            });
        }
        
    }
}