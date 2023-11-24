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
        public void UpdateWeaponView(int count, int extraCount, bool isShowAmmoCounter = true)
        {
            _weaponCounter.SetCount(count, extraCount, isShowAmmoCounter);
        }

        //change weapon
        public void UpdateWeaponView(int count, int extraCount, Sprite icon, bool isShowAmmoCounter = true)
        {
            UpdateWeaponView(count, extraCount, isShowAmmoCounter);
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