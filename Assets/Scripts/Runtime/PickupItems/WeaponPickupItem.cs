using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(SphereCollider))]
    public class WeaponPickupItem : MonoBehaviour, IPickupItem
    {
        [SerializeField] private WeaponType _type;
        [SerializeField, Min(1)] private int _amount;

        public void Pickup(IPickupVisitor visitor)
        {
            visitor.PickupWeapon(_type, _amount);
            Reclaim();
        }

        private void Reclaim() => Destroy(gameObject);
    }
}
