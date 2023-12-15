using System;
using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class WeaponReloadSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;
        private EcsWorld _world;
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

            _world = systems.GetWorld();
            _weaponPool = _world.GetPool<WeaponComponent>();
            _ownerPool = _world.GetPool<WeaponOwnerComponent>();
            _ammoPool = _world.GetPool<AmmunitionComponent>();
            _evtPool = _world.GetPool<WeaponReloadEvent>();           
        }


        public void Run(IEcsSystems systems)
        {                      
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
                        weapon.WeaponReadyTime = Time.time + timeReload;
                    }

                    //update ui event
                    _world.GetOrAddComponent<WeaponChangeStateComponentTag>(ent); 
                }
            }
        }


        private bool IsReloadCompleted(ref WeaponComponent weapon, ref AmmunitionComponent ammo)
        {
            var need = ammo.MaxAmmo - ammo.Count;

            if (weapon.View.Settings.IsRangeWeapon && 
                need > 0 && 
                ammo.ExtraCount > 0 || IsInfinityAmmo())
            {
                if (IsInfinityAmmo())
                {
                    ammo.Count += need;
                    ammo.ExtraCount = 0;
                }
                else if (ammo.ExtraCount >= need)
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


        private bool IsInfinityAmmo()
        {
            return ServicesPool.Instance.GetService<GameConfig>().IsAmmoInfinity; 
        }
    }
}