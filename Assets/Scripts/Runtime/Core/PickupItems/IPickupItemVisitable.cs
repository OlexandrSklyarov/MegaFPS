
namespace SA.FPS
{
    public interface IPickupItemVisitable
    {
        void Pickup(IPickupVisitor visitor);
    }
}