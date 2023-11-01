using System.Collections.Generic;
using UnityEngine;
namespace SA.FPS.Runtime.UI.HUD
{
    public class UIWeaponCollectionView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private UIWeaponView _prefab;

        public void Awake() => ClearContent();

        public void SetWeaponCollection(IEnumerable<UIWeaponItemView> views)
        {
            ClearContent();

            foreach(var view in views)
            {
                Util.DebugUtil.PrintColor($"{view.Icon.name} is used: {view.IsUsed}", Color.cyan);
                var item = Instantiate(_prefab, _content);
                item.SetIcon(view.Icon, view.IsUsed);
            }
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