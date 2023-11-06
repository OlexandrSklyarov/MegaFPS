using System.Collections.Generic;
using SA.FPS.Runtime.UI.HUD;
using UnityEngine;

namespace SA.FPS
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIWeaponCounter _weaponCounter;
        [SerializeField] private UIWeaponCounter _grenadeCounter;  
        [SerializeField] private UIWeaponCollectionView _weaponCollectionView;  
        
        //weapon count
        public void UpdateWeaponView(int count, int extraCount)
        {
            _weaponCounter.SetCount(count, extraCount);
        }

        //change weapon
        public void UpdateWeaponView(int count, int extraCount, Sprite icon)
        {
            UpdateWeaponView(count, extraCount);
            _weaponCounter.SetIcon(icon);
        }
        

        /// <summary>
        /// Update weapons collection icon
        /// </summary>
        /// <param name="views"></param> <summary>
        /// 
        /// </summary>
        /// <param name="views"></param>
        public void UpdateWeaponCollection(IEnumerable<UIWeaponItemView> views)
        {
            _weaponCollectionView.SetWeaponCollection(views);
        }
    }
}