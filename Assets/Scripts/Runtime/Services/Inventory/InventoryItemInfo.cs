using UnityEngine;

namespace Runtime.Services.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "SO/Inventory/InventoryItemInfo", order = 0)]
    public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
    {
        [field: SerializeField] public string Id {get; private set;}
        [field: SerializeField] public string Title {get; private set;}
        [field: SerializeField] public string Description {get; private set;}
        [field: SerializeField] public int MaxItemsInInventorySlot {get; private set;}
        [field: SerializeField] public Sprite Icon {get; private set;}
    }
}