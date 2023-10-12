using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponUpdateStateSystem: IEcsInitSystem, IEcsRunSystem
    {
        private HUDController _uiController;
        private EcsFilter _weaponFilter;   

        public void Init(IEcsSystems systems)
        {     
            _uiController = systems.GetShared<SharedData>().WorldData.HUD;

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<WeaponChangeStateComponentTag>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var ammoPool = world.GetPool<AmmunitionComponent>();
            var updateStatePool = world.GetPool<WeaponChangeStateComponentTag>();

            //EVENT
            foreach(var ent in _weaponFilter)
            {
                ref var ammo = ref ammoPool.Get(ent);
                
                _uiController.UpdateWeaponPanel(ammo.Count);

                updateStatePool.Del(ent);
            }
        }        
    }
}