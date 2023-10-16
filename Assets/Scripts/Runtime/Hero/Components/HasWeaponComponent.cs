using System.Collections.Generic;
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public struct HasWeaponComponent
    {        
        public Dictionary<WeaponType, int> MyWeapons;        
        public WeaponType CurrentWeaponType;
    }
}