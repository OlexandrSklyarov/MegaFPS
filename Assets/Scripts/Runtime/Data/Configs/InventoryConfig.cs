using Runtime.Services.Inventory;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "SO/InventoryConfig")]
    public sealed class InventoryConfig : ScriptableObject   
    {
        [field: SerializeField] public int MaxSlots {get; private set;} = 5;
        [field: SerializeField] public InventoryItemInfo[] Items {get; private set;}
    }
}
