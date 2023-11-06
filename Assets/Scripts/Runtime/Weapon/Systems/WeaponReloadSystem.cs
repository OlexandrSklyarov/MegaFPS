using Leopotam.EcsLite;
using Runtime.Extensions;

namespace SA.FPS
{
    public sealed class WeaponReloadSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;
        private EcsPool<AmmunitionComponent> _ammoPool;
        private EcsPool<WeaponReloadEvent> _evtPool;

        public void Init(IEcsSystems systems)
        {
            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<WeaponReloadEvent>()
                .End();

            var world = systems.GetWorld();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
            _ammoPool = world.GetPool<AmmunitionComponent>();
            _evtPool = world.GetPool<WeaponReloadEvent>();
        }


        public void Run(IEcsSystems systems)
        {          
            var world = systems.GetWorld();
            
            //EVENT
            foreach(var ent in _weaponFilter)
            {
                _evtPool.Del(ent);  

                ref var weapon = ref _weaponPool.Get(ent);  
                ref var ammo = ref _ammoPool.Get(ent);  

                if (IsReloadCompleted(ref weapon, ref ammo))
                {
                    if (weapon.View.TryReload(out float timeReload))
                    {
                        weapon.CurrentCooldown = timeReload;
                    }

                    //update ui event
                    world.GetOrAddComponent<WeaponChangeStateComponentTag>(ent); 
                }
            }
        }


        private bool IsReloadCompleted(ref WeaponComponent weapon, ref AmmunitionComponent ammo)
        {
            var need = ammo.MaxAmmo - ammo.Count;

            if (!weapon.View.Settings.IHandWeapon && 
                need > 0 && 
                ammo.ExtraCount > 0)
            {
                if (ammo.ExtraCount >= need)
                {
                    ammo.Count += need;
                    ammo.ExtraCount -= need;
                }
                else
                {
                    ammo.Count += ammo.ExtraCount;
                    ammo.ExtraCount = 0;
                }                

                return true;
            } 

            return false;
        }
    }
}