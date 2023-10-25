using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(SphereCollider))]
    public class WeaponPickupItem : MonoBehaviour, IPickupItemVisitable
    {
        public WeaponType Type => _type;
        public int Amount => _amount;

        [SerializeField] private WeaponType _type;
        [SerializeField, Min(1)] private int _amount;

        public void Pickup(IPickupVisitor visitor)
        {
            visitor.Visit(this);
            Reclaim();
        }

        private void Reclaim() => Destroy(gameObject);
    }
}
