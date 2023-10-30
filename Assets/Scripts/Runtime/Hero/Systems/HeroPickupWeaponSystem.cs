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

            if (!hasWeaponComp.MyWeaponEntities.TryGetValue(evt.Type, out int weaponEntity)) return;

            ref var weaponAmmo = ref world.GetOrAddComponent<AmmunitionComponent>(weaponEntity);
            weaponAmmo.Count += evt.Amount;
            weaponAmmo.Count = Mathf.Min(weaponAmmo.Count, weaponAmmo.MaxAmmo);

            //update weapon event
            world.GetPool<WeaponChangeStateComponentTag>().Add(weaponEntity);
        }


        private bool TryHeroTakeWeaponEvent(EcsWorld world, int heroEntity, ref CharacterPickupWeaponEvent pickupEvent, ref HeroComponent hero)
        {            
            ref var hasWeapon = ref GetHasWeaponComponent(world, heroEntity);   

            var handsTargets = hero.View.HandsWeaponTargetView;  
            
            //weapon not found
            if (!IsHasWeaponInInventory(pickupEvent.Type, ref hasWeapon))
            {
                //try hide previous weapon
                foreach(Transform curWeapon in handsTargets.WeaponsRoot)  
                    curWeapon.gameObject.SetActive(false);                       
                
                //new weapon
                var (weapon, settings) = _weaponFactory.CreateWeaponItem(pickupEvent.Type, handsTargets.WeaponsRoot);
                handsTargets.SetTargets(weapon);
                var weaponEntity = CreateWeaponEntity(world, handsTargets, settings, heroEntity);  

                //save weapon
                hasWeapon.CurrentWeaponType = pickupEvent.Type;
                hasWeapon.MyWeaponEntities.Add(pickupEvent.Type, weaponEntity);

                return true;
            }

            return false;
        }


        private bool IsHasWeaponInInventory(WeaponType type, ref HasWeaponComponent hasWeapon)
        {
            return hasWeapon.MyWeaponEntities.TryGetValue(type, out var value);
        }


        private ref HasWeaponComponent GetHasWeaponComponent(EcsWorld world, int heroEntity)
        {
            ref var hasWeapon = ref world.GetOrAddComponent<HasWeaponComponent>(heroEntity);
            hasWeapon.MyWeaponEntities ??= new Dictionary<WeaponType, int>();
            return ref hasWeapon;
        }

        private int CreateWeaponEntity(EcsWorld world, HandsWeaponTargetView handsWeaponTargetView, WeaponSettings settings, int ownerEntity)
        {
            var ent = world.NewEntity();
            
            //weapon
            ref var weapon = ref world.GetPool<WeaponComponent>().Add(ent);            
            weapon.Settings = settings;
            weapon.FirePoint = handsWeaponTargetView.FirePoint;
            weapon.Center = handsWeaponTargetView.transform;

            //ammo
            ref var ammunition = ref world.GetPool<AmmunitionComponent>().Add(ent); 
            ammunition.Count = settings.StartAmmo;
            ammunition.MaxAmmo = settings.StartAmmo;
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