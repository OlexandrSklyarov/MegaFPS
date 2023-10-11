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
        [field: SerializeField] public Camera HeroCamera {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}

        private int _heroEntity;
        private EcsWorld _world;

        public FireWeaponView[] WeaponViews {get; private set;}
        public Transform FollowTarget => transform;

        public void Init(int entity, EcsWorld world)
        {
            _heroEntity = entity;
            _world = world;
            WeaponViews = GetComponentsInChildren<FireWeaponView>(true);
        }

        private void OnTriggerEnter(Collider other) 
        {
            DebugUtility.PrintColor($"OnTrigger obj: {other.name}", Color.yellow);   
            if (other.TryGetComponent(out BasePickupItem item))
            {
                ref var evt = ref _world.GetPool<CharacterPickupWeaponEvent>().Add(_heroEntity);
                evt.Type = item.Type;
                evt.Amount = item.Amount;

                item.Reclaim();
            } 
        }
    }
}