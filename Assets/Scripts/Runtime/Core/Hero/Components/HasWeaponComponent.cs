using System.Collections.Generic;
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public struct HasWeaponComponent
    {        
        public Dictionary<WeaponType, int> MyWeaponCollections;        
        public WeaponType CurrentUsedWeaponType;
        public float NextSwitchTime;
        public float NextReloadTime;
    }
}