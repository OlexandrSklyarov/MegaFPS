using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

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
                _tryShootEvtPool.Del(ent); 

                ref var weapon = ref _weaponPool.Get(ent);
                ref var owner = ref _ownerPool.Get(ent);
                ref var ammo = ref _ammoPool.Get(ent);                 

                if (weapon.CurrentCooldown > 0f)
                {
                    weapon.CurrentCooldown -= Time.deltaTime;
                    continue;
                }
                                    
                //fire
                Shoot(ref weapon);
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
        }


        private void Shoot(ref WeaponComponent weapon)
        {
            Util.DebugUtility.Print("Shoot");
            FMODUnity.RuntimeManager.PlayOneShot(weapon.Settings.FireSfx);   
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