using System.Linq;
using Leopotam.EcsLite;
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
            foreach(var ent in _filter)
            {
                _eventPool.Del(ent);

                ref var hero = ref _heroPool.Get(ent);
                ref var hasWeapon = ref _hasWeaponPool.Get(ent);

                if (hasWeapon.LastSwitchTime > Time.time) continue;
                if (hasWeapon.MyWeaponCollections.Count < 2) continue;

                var nextWeaponType = GetNextWeaponType(ref hasWeapon);

                var handsTargets = hero.View.HandsWeaponTargetView;  

                //try hide previous weapon
                foreach(Transform curWeapon in handsTargets.WeaponsRoot)  
                {
                    curWeapon.gameObject.SetActive(false);
                }

                //new weapon
                var (weapon, settings) = _weaponFactory.CreateWeaponItem(nextWeaponType, handsTargets.WeaponsRoot);
                handsTargets.SetTargets(weapon); 
                
                hasWeapon.CurrentUsedWeaponType = nextWeaponType;
                hasWeapon.LastSwitchTime = Time.time + 0.5f;
            }
        }

        private WeaponType GetNextWeaponType(ref HasWeaponComponent hasWeapon)
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

            return collection[index].Key;
        }
    }
}