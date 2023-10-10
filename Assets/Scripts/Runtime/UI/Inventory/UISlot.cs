using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI.Inventory
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        public virtual void OnDrop(PointerEventData eventData)
        {
            var otherItem = eventData.pointerDrag.transform;
            otherItem.SetParent(transform);
            otherItem.localPosition = Vector3.zero;
        }        
    }
}
