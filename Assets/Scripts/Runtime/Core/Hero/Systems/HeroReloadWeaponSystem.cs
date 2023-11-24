using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroReloadWeaponSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<HasWeaponComponent> _weaponPool;
        private EcsPool<CharacterReloadWeaponEvent> _eventPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HasWeaponComponent>()
                .Inc<CharacterReloadWeaponEvent>()
                .End();

            var world = systems.GetWorld();
            _weaponPool = world.GetPool<HasWeaponComponent>();
            _eventPool = world.GetPool<CharacterReloadWeaponEvent>();
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                _eventPool.Del(ent);

                ref var weapon = ref _weaponPool.Get(ent);

                if (weapon.NextReloadTime > Time.time) continue;

                var weaponEntity = weapon.MyWeaponCollections[weapon.CurrentUsedWeaponType];
                world.GetOrAddComponent<WeaponReloadEvent>(weaponEntity); 

                weapon.NextReloadTime = Time.time + 1f;
            }
        }
    }
}