using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public sealed class HeroView : MonoBehaviour
    {
        [field: SerializeField] public CharacterConfig Config {get; private set;}
        [field: SerializeField] public CharacterController CharacterController {get; private set;}
        [field: SerializeField] public Transform LookTarget {get; private set;}
        [field: SerializeField] public Transform HeadRoot {get; private set;}
        [field: SerializeField] public Transform Head {get; private set;}
        [field: SerializeField] public Transform WeaponsRoot {get; private set;}
        [field: SerializeField] public Camera HeroCamera {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}

        public Transform FollowTarget => transform;

        private EcsPackedEntity _heroEntity;
        private EcsWorld _world;
        private WorldItemInteractor _worldItemInteractor;


        public void Init(int entity, EcsWorld world)
        {            
            _heroEntity = world.PackEntity(entity);
            _world = world;

            _worldItemInteractor = new WorldItemInteractor(_world, _heroEntity);
        }


        private void OnTriggerEnter(Collider other) 
        {
            DebugUtility.PrintColor($"OnTrigger obj: {other.name}", Color.yellow);   
            
            if (other.TryGetComponent(out IPickupItemVisitable item))
            {      
                item.Pickup(_worldItemInteractor);
            } 
        }
    }
}