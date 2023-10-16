using Leopotam.EcsLite;
using Runtime.Services.WeaponsFactory;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public sealed class HeroView : MonoBehaviour, IPickupVisitor
    {
        [field: SerializeField] public CharacterConfig Config {get; private set;}
        [field: SerializeField] public CharacterController CharacterController {get; private set;}
        [field: SerializeField] public Transform LookTarget {get; private set;}
        [field: SerializeField] public Transform HeadRoot {get; private set;}
        [field: SerializeField] public Transform Head {get; private set;}
        [field: SerializeField] public Camera HeroCamera {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}

        private EcsPackedEntity _heroEntity;
        private EcsWorld _world;

        public FireWeaponView[] WeaponViews {get; private set;}
        public Transform FollowTarget => transform;

        public void Init(int entity, EcsWorld world)
        {
            _heroEntity = world.PackEntity(entity);
            _world = world;
            WeaponViews = GetComponentsInChildren<FireWeaponView>(true);
        }


        private void OnTriggerEnter(Collider other) 
        {
            DebugUtility.PrintColor($"OnTrigger obj: {other.name}", Color.yellow);   
            if (other.TryGetComponent(out IPickupItem item))
            {      
                item.Pickup(this);
            } 
        }


    #region Pickup methods
        void IPickupVisitor.PickupWeapon(WeaponType type, int amount)
        {
            if (!_heroEntity.Unpack(_world, out int ent)) return;

            ref var evt = ref _world.GetPool<CharacterPickupWeaponEvent>().Add(ent);
            evt.Type = type;
            evt.Amount = amount;
        }
    #endregion
    }
}