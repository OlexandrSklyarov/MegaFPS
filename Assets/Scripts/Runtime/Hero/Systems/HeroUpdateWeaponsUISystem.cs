using System.Collections.Generic;
using Leopotam.EcsLite;
using Runtime.Extensions;
using SA.FPS.Runtime.UI.HUD;

namespace SA.FPS
{
    public sealed class HeroUpdateWeaponsUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private HUDController _uiController;
        private EcsFilter _filter;
        private EcsPool<HasWeaponComponent> _hasWeaponPool;
        private EcsPool<UpdateWeaponsViewEvent> _eventPool;

        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            _uiController = data.WorldData.HUD;

            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HasWeaponComponent>()
                .Inc<UpdateWeaponsViewEvent>()
                .End();

            var world = systems.GetWorld();
            _hasWeaponPool = world.GetPool<HasWeaponComponent>();
            _eventPool = world.GetPool<UpdateWeaponsViewEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            foreach(var ent in _filter)
            {
                _eventPool.Del(ent);

                ref var hasWeapon = ref _hasWeaponPool.Get(ent);

                var weaponCollection = new List<UIWeaponItemView>();

                foreach(var item in hasWeapon.MyWeaponCollections)
                {
                    if (item.Key == hasWeapon.CurrentUsedWeaponType) continue;

                    var entity = item.Value;
                    ref var weapon = ref world.GetOrAddComponent<WeaponComponent>(entity);

                    weaponCollection.Add(new UIWeaponItemView
                    {
                        Icon = weapon.Settings.Sprite
                    });
                }

                _uiController.UpdateWeaponCollection(weaponCollection);
            }
        }
    }
}