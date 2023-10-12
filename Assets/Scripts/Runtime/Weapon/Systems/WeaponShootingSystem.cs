using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class WeaponShootingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;  

        public void Init(IEcsSystems systems)
        {     
            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<TryShootComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var tryShootEvtPool = world.GetPool<TryShootComponent>();
            var weaponPool = world.GetPool<WeaponComponent>();
            var ownerPool = world.GetPool<WeaponOwnerComponent>();
            var ammoPool = world.GetPool<AmmunitionComponent>();

            //WEAPON
            foreach(var ent in _weaponFilter)
            {
                tryShootEvtPool.Del(ent); 

                ref var weapon = ref weaponPool.Get(ent);
                ref var owner = ref ownerPool.Get(ent);
                ref var ammo = ref ammoPool.Get(ent);                 

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
                    ammoPool.Del(ent);
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