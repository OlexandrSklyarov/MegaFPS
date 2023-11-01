using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponUpdateStateSystem: IEcsInitSystem, IEcsRunSystem
    {
        private HUDController _uiController;
        private EcsFilter _filter;
        private EcsPool<AmmunitionComponent> _ammoPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponChangeStateComponentTag> _updateStatePool;

        public void Init(IEcsSystems systems)
        {     
            _uiController = systems.GetShared<SharedData>().WorldData.HUD;

            _filter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<WeaponChangeStateComponentTag>()
                .End();            

            var world = systems.GetWorld();
            _ammoPool = world.GetPool<AmmunitionComponent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _updateStatePool = world.GetPool<WeaponChangeStateComponentTag>();

            _uiController.UpdateWeaponView(0);
        }

        public void Run(IEcsSystems systems)
        {            
            //Ammo amount event
            foreach(var ent in _filter)
            {
                _updateStatePool.Del(ent);

                ref var ammo = ref _ammoPool.Get(ent);
                ref var weapon = ref _weaponPool.Get(ent);
                
                _uiController.UpdateWeaponView(ammo.Count, weapon.Settings.Sprite);
            }
        }        
    }
}