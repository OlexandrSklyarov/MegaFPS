using SA.FPS;
using UnityEngine;

namespace Runtime.Services.WeaponsFactory
{
    public interface IWeaponItemFactory : IService
    {
        WeaponView GetWeaponItem(WeaponType type, Transform weaponsRoot);
    }
}