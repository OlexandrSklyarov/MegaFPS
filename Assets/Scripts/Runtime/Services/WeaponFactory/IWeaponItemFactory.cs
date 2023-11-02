using SA.FPS;
using UnityEngine;

namespace Runtime.Services.WeaponsFactory
{
    public interface IWeaponItemFactory : IService
    {
        (IWeaponView, WeaponSettings) CreateWeaponItem(WeaponType type, Transform weaponsRoot);
    }
}