using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using SA.FPS.Runtime.UI.HUD;
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
            var data = systems.GetShared<SharedData>();
            _weaponFactory = data.Services.GetService<IWeaponItemFactory>();
        
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

                ref var hasWeapon = ref GetHasWeaponComponent(world, ent);               

                if (!TryTakeNewWeapon(world, ent, ref hasWeapon, ref evt, ref hero))
                {
                    TryAddAmmo(world, ref hasWeapon, ref evt);  
                }

                UpdateWeaponsUI(world, ent);   

                _evtPool.Del(ent);
            }
        }


        private void UpdateWeaponsUI(EcsWorld world, int heroEntity)
        {
            world.GetOrAddComponent<UpdateWeaponsViewEvent>(heroEntity);
        }


        private void TryAddAmmo(EcsWorld world, ref HasWeaponComponent hasWeapon, ref CharacterPickupWeaponEvent evt)
        {
            if (!hasWeapon.MyWeaponCollections.TryGetValue(evt.Type, out int weaponEntity)) return;

            ref var weaponAmmo = ref world.GetOrAddComponent<AmmunitionComponent>(weaponEntity);
            weaponAmmo.Count += evt.Amount;
            weaponAmmo.Count = Mathf.Min(weaponAmmo.Count, weaponAmmo.MaxAmmo);

            //update weapon event
            world.GetPool<WeaponChangeStateComponentTag>().Add(weaponEntity);
        }


        private bool TryTakeNewWeapon(EcsWorld world, int heroEntity, ref HasWeaponComponent hasWeapon,
            ref CharacterPickupWeaponEvent pickupEvent, ref HeroComponent hero)
        {            
            var isTakeNewWeapon = false;

            var handsTargets = hero.View.HandsWeaponTargetView;  

            //try hide previous weapon
            foreach(Transform curWeapon in handsTargets.WeaponsRoot)  
            {
                curWeapon.gameObject.SetActive(false);
            }

            //new weapon
            var (weapon, settings) = _weaponFactory.CreateWeaponItem(pickupEvent.Type, handsTargets.WeaponsRoot);
            handsTargets.SetTargets(weapon);
            
            //weapon not found
            if (!IsHasWeaponInInventory(pickupEvent.Type, ref hasWeapon))
            {                
                CreateWeaponEntity(world, handsTargets, settings, heroEntity, ref hasWeapon, ref pickupEvent);                  
                
                isTakeNewWeapon = true;
            }

            hasWeapon.CurrentUsedWeaponType = pickupEvent.Type;

            return isTakeNewWeapon;
        }


        private bool IsHasWeaponInInventory(WeaponType type, ref HasWeaponComponent hasWeapon)
        {
            return hasWeapon.MyWeaponCollections.TryGetValue(type, out var value);
        }


        private ref HasWeaponComponent GetHasWeaponComponent(EcsWorld world, int heroEntity)
        {
            ref var hasWeapon = ref world.GetOrAddComponent<HasWeaponComponent>(heroEntity);
            hasWeapon.MyWeaponCollections ??= new Dictionary<WeaponType, int>();
            return ref hasWeapon;
        }


        private void CreateWeaponEntity(EcsWorld world, HandsWeaponTargetView handsWeaponTargetView, 
            WeaponSettings settings, int ownerEntity, ref HasWeaponComponent hasWeapon, 
            ref CharacterPickupWeaponEvent pickupEvent)
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

            //add to inventory
            hasWeapon.MyWeaponCollections.Add(pickupEvent.Type, ent);
        }
    }
}