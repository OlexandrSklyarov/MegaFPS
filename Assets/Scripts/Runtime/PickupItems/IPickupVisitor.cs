
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public interface IPickupVisitor
    {
        void Visit(WeaponPickupItem weapon);
    }
}