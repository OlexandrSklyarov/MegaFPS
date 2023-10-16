
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public interface IPickupVisitor
    {
        void PickupWeapon(WeaponType type, int amount);
    }
}