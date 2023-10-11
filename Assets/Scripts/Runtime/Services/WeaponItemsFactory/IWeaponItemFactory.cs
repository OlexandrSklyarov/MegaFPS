using Runtime.Services.Inventory;
using SA.FPS;

namespace Runtime.Services.WeaponsFactory
{
    public interface IWeaponItemFactory : IService
    {
        IInventoryItem CreateWeaponItem(WeaponType type);
    }
}