
namespace SA.FPS
{
    public interface IPickupItem
    {
        void Pickup(IPickupVisitor visitor);
    }
}