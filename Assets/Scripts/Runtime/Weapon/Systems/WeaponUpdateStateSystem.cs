using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponUpdateStateSystem: IEcsInitSystem, IEcsRunSystem
    {
        private HUDController _uiController;
        private EcsFilter _weaponFilter;
        private EcsPool<AmmunitionComponent> _ammoPool;
        private EcsPool<WeaponChangeStateComponentTag> _updateStatePool;

        public void Init(IEcsSystems systems)
        {     
            _uiController = systems.GetShared<SharedData>().WorldData.HUD;

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<WeaponChangeStateComponentTag>()
                .End();

            var world = systems.GetWorld();
            _ammoPool = world.GetPool<AmmunitionComponent>();
            _updateStatePool = world.GetPool<WeaponChangeStateComponentTag>();
        }

        public void Run(IEcsSystems systems)
        {            
            //EVENT
            foreach(var ent in _weaponFilter)
            {
                ref var ammo = ref _ammoPool.Get(ent);
                
                _uiController.UpdateWeaponPanel(ammo.Count);

                _updateStatePool.Del(ent);
            }
        }        
    }
}