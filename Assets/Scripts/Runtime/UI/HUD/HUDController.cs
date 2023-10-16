using SA.FPS.Runtime.UI.HUD;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIWeaponCounter _weaponCounter;
        [SerializeField] private UIWeaponCounter _grenadeCounter;     
        
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