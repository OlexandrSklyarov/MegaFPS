using System.Collections.Generic;
using Leopotam.EcsLite;
using Runtime.Extensions;
using SA.FPS.Runtime.UI.HUD;

namespace SA.FPS
{
    public sealed class UpdateHUDSystem : IEcsInitSystem, IEcsRunSystem
    {
        private HUDController _uiController;
        private EcsFilter _weaponFilter;
        private EcsFilter _heroFilter;
        private EcsPool<AmmunitionComponent> _ammoPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponChangeStateComponentTag> _weaponUpdateStatePool;
        private EcsPool<HasWeaponComponent> _hasWeaponPool;
        private EcsPool<UpdateWeaponsViewEvent> _updateViewInventoryEvent;

        public void Init(IEcsSystems systems)
        {     
            _uiController = systems.GetShared<SharedData>().WorldData.HUD;

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<WeaponChangeStateComponentTag>()
                .End();  

            _heroFilter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HasWeaponComponent>()
                .Inc<UpdateWeaponsViewEvent>()
                .End();          

            var world = systems.GetWorld();

            _ammoPool = world.GetPool<AmmunitionComponent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _weaponUpdateStatePool = world.GetPool<WeaponChangeStateComponentTag>();
            _hasWeaponPool = world.GetPool<HasWeaponComponent>();
            _updateViewInventoryEvent = world.GetPool<UpdateWeaponsViewEvent>();

            _uiController.UpdateWeaponView(0);
        }

        public void Run(IEcsSystems systems)
        {            
            var world = systems.GetWorld();

            //weapon
            foreach(var ent in _weaponFilter)
            {
                _weaponUpdateStatePool.Del(ent);

                ref var ammo = ref _ammoPool.Get(ent);
                ref var weapon = ref _weaponPool.Get(ent);
                
                _uiController.UpdateWeaponView(ammo.Count, weapon.Settings.Sprite);
            }

            //hero
            foreach(var ent in _heroFilter)
            {
                _updateViewInventoryEvent.Del(ent);

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