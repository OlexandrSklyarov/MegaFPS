using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [RequireComponent(typeof(SphereCollider))]
    public class BasePickupItem : MonoBehaviour
    {
        [field: SerializeField] public WeaponType Type {get; private set;}
        [field: SerializeField, Min(1)] public int Amount {get; private set;}
        
        public void Reclaim() => Destroy(gameObject);
    }
}
