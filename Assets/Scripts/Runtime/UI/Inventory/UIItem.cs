using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UI.Inventory
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        private void Awake() 
        {
            _rectTransform = GetComponent<RectTransform>();  
            _canvas = GetComponentInParent<Canvas>();  
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var slotTransform = _rectTransform.parent;
            slotTransform.SetAsLastSibling();
            _canvasGroup.alpha = 0.7f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

        }
    }
}
