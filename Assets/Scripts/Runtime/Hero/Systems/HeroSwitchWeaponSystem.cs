using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroSwitchWeaponSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IWeaponItemFactory _weaponFactory;
        private EcsFilter _filter;
        private EcsPool<HeroComponent> _heroPool;
        private EcsPool<HasWeaponComponent> _hasWeaponPool;
        private EcsPool<SwitchWeaponEvent> _eventPool;

        public void Init(IEcsSystems systems)
        {
            _weaponFactory = ServicesPool.Instance.GetService<IWeaponItemFactory>();
            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HasWeaponComponent>()
                .Inc<SwitchWeaponEvent>()
                .End();

            var world = systems.GetWorld();
            _heroPool = world.GetPool<HeroComponent>();
            _hasWeaponPool = world.GetPool<HasWeaponComponent>();
            _eventPool = world.GetPool<SwitchWeaponEvent>();
        }

        public void Run(IEcsSystems systems)
        {            
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                _eventPool.Del(ent);

                ref var hero = ref _heroPool.Get(ent);
                ref var hasWeapon = ref _hasWeaponPool.Get(ent);

                if (hasWeapon.NextSwitchTime > Time.time) continue;
                if (hasWeapon.MyWeaponCollections.Count < 2) continue;

                var (weaponType, weaponEntity) = GetNextWeaponType(ref hasWeapon);

                var handsTargets = hero.View.HandsWeaponTargetView;  

                //try hide previous weapon
                foreach(Transform curWeapon in handsTargets.WeaponsRoot)  
                {
                    curWeapon.gameObject.SetActive(false);
                }

                //new weapon
                var weaponView = _weaponFactory.CreateWeaponItem(weaponType, handsTargets.WeaponsRoot);
                handsTargets.SetTargets(weaponView); 
                
                hasWeapon.CurrentUsedWeaponType = weaponType;
                hasWeapon.NextSwitchTime = Time.time + 0.5f;

                //add update ui event for weapon
                world.GetOrAddComponent<WeaponChangeStateComponentTag>(weaponEntity);

                //add update ui event for Hero
                world.GetOrAddComponent<UpdateWeaponsViewEvent>(ent);
            }
        }

        private (WeaponType weaponType, int weaponEntity) GetNextWeaponType(ref HasWeaponComponent hasWeapon)
        {
            var collection = hasWeapon.MyWeaponCollections.ToList();
            var index = 0;
            
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Key == hasWeapon.CurrentUsedWeaponType)
                {
                    index = i;
                    break;
                }
            }

            ++index;
            index %= collection.Count;

            return (collection[index].Key, collection[index].Value);
        }
    }
}