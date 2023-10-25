using SA.FPS;
using UnityEngine;

namespace Runtime.Services.WeaponsFactory
{
    public interface IWeaponItemFactory : IService
    {
        FireWeaponView CreateWeaponItem(WeaponType type, Transform weaponsRoot);
    }
}