using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(SphereCollider))]
    public class WeaponPickupItem : MonoBehaviour, IPickupItemVisitable
    {
        public WeaponType Type => _type;
        public int Amount => _settings.StartAmmo;

        [SerializeField] private WeaponType _type;
        [SerializeField] private WeaponSettings _settings;

        public void Pickup(IPickupVisitor visitor)
        {
            visitor.Visit(this);
            Reclaim();
        }

        private void Reclaim() => Destroy(gameObject);
    }
}
