using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
namespace SA.FPS.Runtime.UI.HUD
{
    public class UIWeaponCollectionView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;        

        private IPoolManager _poolManager;

        public void Awake() 
        {
            _poolManager = ServicesPool.Instance.GetService<IPoolManager>();
            ClearContent();
        } 

        public void SetWeaponCollection(IEnumerable<UIWeaponItemView> views)
        {
            ClearContent();

            foreach(var view in views)
            {
                var item = GetItem();
                item.SetIcon(view.Icon, view.IsUsed);
            }
        }

        private UIWeaponView GetItem()
        {
            var item = _poolManager.GetUIWeaponView();
            item.transform.SetParent(_content);
            item.transform.localScale = Vector3.one;
            return item;
        }

        private void ClearContent()
        {
            foreach(Transform item in _content)
            {
                Destroy(item.gameObject);
            }
        }
    }
}