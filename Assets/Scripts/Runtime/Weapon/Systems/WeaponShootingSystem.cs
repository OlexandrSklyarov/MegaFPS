using System;
using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public sealed class WeaponShootingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;
        private EcsPool<TryShootComponent> _tryShootEvtPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;
        private EcsPool<AmmunitionComponent> _ammoPool;

        public void Init(IEcsSystems systems)
        {     
            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<TryShootComponent>()
                .End();

            var world = systems.GetWorld();
            _tryShootEvtPool = world.GetPool<TryShootComponent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
            _ammoPool = world.GetPool<AmmunitionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            //WEAPON
            foreach(var ent in _weaponFilter)
            {
                ref var weapon = ref _weaponPool.Get(ent);
                ref var owner = ref _ownerPool.Get(ent);
                ref var ammo = ref _ammoPool.Get(ent);                 
                ref var shootEvt = ref _tryShootEvtPool.Get(ent);                 

                if (weapon.CurrentCooldown > 0f) //cooldown
                {
                    weapon.CurrentCooldown -= Time.deltaTime;                    
                }
                else //fire
                {
                    
                    Shoot(ref weapon, ref shootEvt);
                    AddOwnerShakeFX(world, ref owner, ref weapon);
                    weapon.CurrentCooldown = weapon.Settings.ShootCooldown;
                    ammo.Count--;

                    //update ui event
                    world.GetOrAddComponent<WeaponChangeStateComponentTag>(ent);
                    
                    //remove ammo
                    if (ammo.Count <= 0)
                    {
                        _ammoPool.Del(ent);
                    }  
                }

                _tryShootEvtPool.Del(ent);                                     
            }         
        }


        private void Shoot(ref WeaponComponent weapon, ref TryShootComponent shootEvt)
        {
            FMODUnity.RuntimeManager.PlayOneShot(weapon.Settings.FireSfx);  

            var dir = AddShootSpread(weapon.Settings.Spread, shootEvt.Direction);

            if (Physics.Raycast(shootEvt.ShootPoint, dir, out var hit, float.MaxValue, weapon.Settings.TargetLayerMask))
            {
                Util.DebugUtility.Print($"Hit!!! -- {hit.collider.name} pos {hit.normal}");
                UnityEngine.Debug.DrawLine(shootEvt.ShootPoint, hit.point, Color.yellow, 0.1f);
            }
        }


        private Vector3 AddShootSpread(Vector3 spread, Vector3 dir)
        {
            var modifierDir =  dir + new Vector3
            (
                UnityEngine.Random.Range(-spread.x, spread.x),
                UnityEngine.Random.Range(-spread.y, spread.y),
                UnityEngine.Random.Range(-spread.z, spread.z)
            );

            return modifierDir.normalized;
        }


        private void AddOwnerShakeFX(EcsWorld world, ref WeaponOwnerComponent owner, ref WeaponComponent weapon)
        {    
            ref var shake = ref world.GetOrAddComponent<CameraShakeComponent>(owner.MyOwnerEntity);  

            shake.Duration = weapon.Settings.ShakeDuration;
            shake.Strength = weapon.Settings.Strength;
            shake.Vibrato = weapon.Settings.Vibrato;
            shake.Randomness = weapon.Settings.Randomness;
        }
    }
}