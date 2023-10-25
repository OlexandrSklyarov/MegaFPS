using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using UnityEngine;
using Util;

namespace SA.FPS
{
    public sealed class HeroPickupWeaponSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IWeaponItemFactory _weaponFactory;
        private EcsFilter _filter;
        private EcsPool<HeroComponent> _heroPool;
        private EcsPool<CharacterPickupWeaponEvent> _evtPool;

        public void Init(IEcsSystems systems)
        {
            _weaponFactory = systems.GetShared<SharedData>().Services.GetService<IWeaponItemFactory>();

            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterPickupWeaponEvent>()
                .End();

            var world = systems.GetWorld();
            _heroPool = world.GetPool<HeroComponent>();
            _evtPool = world.GetPool<CharacterPickupWeaponEvent>();
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                ref var hero = ref _heroPool.Get(ent);
                ref var evt = ref _evtPool.Get(ent);                

                if (!TryHeroTakeWeaponEvent(world, ent, ref evt, ref hero))
                {
                    DebugUtility.Print($"Try add ammo: {evt.Type}");
                    TryAddAmmo(world, ent, ref evt);  
                }
                else 
                {
                    DebugUtility.Print($"Pickup weapon {evt.Type}");                                      
                }

                _evtPool.Del(ent);
            }
        }


        private void TryAddAmmo(EcsWorld world, int heroEntity, ref CharacterPickupWeaponEvent evt)
        {
            ref var hasWeaponComp = ref GetHasWeaponComponent(world, heroEntity); 

            if (!hasWeaponComp.MyWeapons.TryGetValue(evt.Type, out int weaponEntity)) return;

            ref var weaponAmmo = ref world.GetOrAddComponent<AmmunitionComponent>(weaponEntity);
            weaponAmmo.Count += evt.Amount;
            weaponAmmo.Count = Mathf.Min(weaponAmmo.Count, weaponAmmo.MaxAmmo);

            //update weapon event
            world.GetPool<WeaponChangeStateComponentTag>().Add(weaponEntity);
        }


        private bool TryHeroTakeWeaponEvent(EcsWorld world, int heroEntity, ref CharacterPickupWeaponEvent evt, ref HeroComponent hero)
        {
            var pickupWeaponType = evt.Type;  

            ref var hasWeapon = ref GetHasWeaponComponent(world, heroEntity);       
            
            //weapon not found
            if (!IsHasWeaponInInventory(pickupWeaponType, ref hasWeapon))
            {
                //try hide previous weapon
                foreach(Transform weapon in hero.View.WeaponsRoot.transform)  
                    weapon.gameObject.SetActive(false);                       
                
                //new weapon
                var newWeaponView = _weaponFactory.CreateWeaponItem(evt.Type, hero.View.WeaponsRoot);
                var weaponEntity = CreateWeaponEntity(world, newWeaponView, heroEntity);  

                //save weapon
                hasWeapon.CurrentWeaponType = pickupWeaponType;
                hasWeapon.MyWeapons.Add(pickupWeaponType, weaponEntity);

                return true;
            }

            return false;
        }


        private bool IsHasWeaponInInventory(WeaponType type, ref HasWeaponComponent hasWeapon)
        {
            return hasWeapon.MyWeapons.TryGetValue(type, out var value);
        }


        private ref HasWeaponComponent GetHasWeaponComponent(EcsWorld world, int heroEntity)
        {
            ref var hasWeapon = ref world.GetOrAddComponent<HasWeaponComponent>(heroEntity);
            hasWeapon.MyWeapons ??= new Dictionary<WeaponType, int>();
            return ref hasWeapon;
        }

        private int CreateWeaponEntity(EcsWorld world, FireWeaponView weaponView, int ownerEntity)
        {
            var ent = world.NewEntity();
            
            //weapon
            ref var weapon = ref world.GetPool<WeaponComponent>().Add(ent);            
            weapon.Settings = weaponView.Settings;
            weapon.FirePoint = weaponView.FirePoint;
            weapon.Center = weaponView.transform;
            weapon.AnimatorRef = weaponView.WeaponAnimator;

            //ammo
            ref var ammunition = ref world.GetPool<AmmunitionComponent>().Add(ent); 
            ammunition.Count = weaponView.Settings.StartAmmo;
            ammunition.MaxAmmo = weaponView.Settings.StartAmmo;
            UnityEngine.Debug.Log(ammunition.Count);            
            
            //owner
            ref var weaponOwner = ref world.GetPool<WeaponOwnerComponent>().Add(ent);
            weaponOwner.MyOwnerEntity = ownerEntity;

            //owner
            world.GetPool<WeaponChangeStateComponentTag>().Add(ent);

            return ent;
        }
    }
}