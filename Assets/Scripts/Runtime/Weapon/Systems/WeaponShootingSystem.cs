using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class WeaponShootingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _shootFilter;  
        private EcsFilter _weaponFilter;  

        public void Init(IEcsSystems systems)
        {            
            _shootFilter = systems.GetWorld()
                .Filter<CharacterTryShootEvent>()
                .End();

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var evtPool = world.GetPool<CharacterTryShootEvent>();
            var weaponPool = world.GetPool<WeaponComponent>();
            var ownerPool = world.GetPool<WeaponOwnerComponent>();
            var ammoPool = world.GetPool<AmmunitionComponent>();

            //EVENT
            foreach(var s in _shootFilter)
            {
                ref var evt = ref evtPool.Get(s);                

                //WEAPON
                foreach(var w in _weaponFilter)
                {
                    ref var weapon = ref weaponPool.Get(w);
                    ref var owner = ref ownerPool.Get(w);
                    ref var ammo = ref ammoPool.Get(w);

                    //skip
                    if (owner.MyOwner != evt.ShootEntity) continue;

                    if (weapon.CurrentCooldown > 0f)
                    {
                        weapon.CurrentCooldown -= Time.deltaTime;
                        continue;
                    }
                                        
                    //fire
                    Shoot(ref weapon);
                    AddOwnerShakeFX(world, ref evt, ref weapon);
                    weapon.CurrentCooldown = weapon.Settings.ShootCooldown;

                    ammo.Count--;
                    
                    //remove ammo
                    if (ammo.Count <= 0)
                    {
                        ammoPool.Del(w);
                    }                    
                }

                evtPool.Del(s);  
            }
        }


        private void Shoot(ref WeaponComponent weapon)
        {
            Util.DebugUtility.Print("Shoot");
            FMODUnity.RuntimeManager.PlayOneShot(weapon.Settings.FireSfx);   
        }


        private void AddOwnerShakeFX(EcsWorld world, ref CharacterTryShootEvent evt, ref WeaponComponent weapon)
        {    
            ref var shake = ref GetShakeComponent(world, ref evt, ref weapon);

            shake.Duration = weapon.Settings.ShakeDuration;
            shake.Strength = weapon.Settings.Strength;
            shake.Vibrato = weapon.Settings.Vibrato;
            shake.Randomness = weapon.Settings.Randomness;
        }


        private ref CameraShakeComponent GetShakeComponent(EcsWorld world, ref CharacterTryShootEvent evt, ref WeaponComponent weapon)
        {
            var shakePool = world.GetPool<CameraShakeComponent>();

            if (shakePool.Has(evt.ShootEntity)) 
            {
                return ref shakePool.Get(evt.ShootEntity);
            }   
            
            return ref shakePool.Add(evt.ShootEntity);            
        }
    }
}