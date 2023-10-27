using SA.FPS.Runtime.UI.HUD;
using UnityEngine;

namespace SA.FPS
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIWeaponCounter _weaponCounter;
        [SerializeField] private UIWeaponCounter _grenadeCounter;     
        
        //weapon count
        public void UpdateWeaponView(int count)
        {
            _weaponCounter.SetCount(count);
        }

        //change weapon
        public void UpdateWeaponView(int count, Sprite icon)
        {
            UpdateWeaponView(count);
            _weaponCounter.SetIcon(icon);
        }
    }
}