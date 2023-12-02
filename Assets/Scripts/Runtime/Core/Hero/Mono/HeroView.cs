using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public sealed class HeroView : MonoBehaviour
    {
        [field: SerializeField] public CharacterConfig Config {get; private set;}
        [field: Space, SerializeField] public Rigidbody RB {get; private set;}
        [field: SerializeField] public Animator HeadAnimator {get; private set;}
        [field: SerializeField] public Transform WeaponsRoot {get; private set;}
                
        [field: Space, SerializeField] public Transform HeadRoot {get; private set;}
        [field: SerializeField] public Transform Head {get; private set;}
        [field: SerializeField] public Transform FPSHeroCameraTarget {get; private set;}
        
        public Transform FollowTarget => transform;

        private EcsPackedEntity _heroEntity;
        private EcsWorld _world;
        private WorldPickupItemInteractor _pickupItemInteractor;


        public void Init(int entity, EcsWorld world)
        {            
            _heroEntity = world.PackEntity(entity);
            _world = world;

            _pickupItemInteractor = new WorldPickupItemInteractor(_world, _heroEntity);
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IPickupItemVisitable item))
            {      
                item.Pickup(_pickupItemInteractor);
            } 
        }
    }
}