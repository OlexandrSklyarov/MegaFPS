using System.Collections.Generic;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

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
            _weaponFactory = ServicesPool.Instance.GetService<IWeaponItemFactory>();
        
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

                TakeWeapon(world, ent, ref hasWeapon, ref evt, ref hero);                

                UpdateWeaponsUI(world, ent);   

                _evtPool.Del(ent);
            }
        }


        private void UpdateWeaponsUI(EcsWorld world, int heroEntity)
        {
            world.GetOrAddComponent<UpdateWeaponsViewEvent>(heroEntity);
        }
       

        private void TakeWeapon(EcsWorld world, int heroEntity, ref HasWeaponComponent hasWeapon,
            ref CharacterPickupWeaponEvent pickupEvent, ref HeroComponent hero)
        {            
            var isTakeNewWeapon = false;

            var weaponsRoot = hero.ViewRef.WeaponsRoot;             
                        
            //weapon not found
            if (!IsHasWeaponInInventory(pickupEvent.Type, ref hasWeapon))
            {                
                HidAllWeapons(weaponsRoot);            
          
                var weaponView = _weaponFactory.GetWeaponItem(pickupEvent.Type, weaponsRoot);

                CreateWeaponEntity(world, weaponView, heroEntity, ref hasWeapon, ref pickupEvent);                  
                
                hasWeapon.CurrentUsedWeaponType = pickupEvent.Type;

                isTakeNewWeapon = true;
            }

            //increase ammo
            if (!isTakeNewWeapon) TryAddAmmo(world, ref hasWeapon, ref pickupEvent);  
        }


        private static void HidAllWeapons(Transform weaponsRoot)
        {
            //try hide previous weapon
            foreach (Transform curWeapon in weaponsRoot)
            {
                curWeapon.gameObject.SetActive(false);
            }
        }


        private bool IsHasWeaponInInventory(WeaponType type, ref HasWeaponComponent hasWeapon)
        {
            return hasWeapon.MyWeaponCollections.TryGetValue(type, out var value);
        }
        

        private void TryAddAmmo(EcsWorld world, ref HasWeaponComponent hasWeapon, ref CharacterPickupWeaponEvent evt)
        {
            if (!hasWeapon.MyWeaponCollections.TryGetValue(evt.Type, out int weaponEntity)) return;

            ref var weaponAmmo = ref world.GetOrAddComponent<AmmunitionComponent>(weaponEntity);

            var needCountForWeapon = weaponAmmo.MaxAmmo - weaponAmmo.Count;
            
            if (needCountForWeapon <= evt.Amount)
            {
                weaponAmmo.Count += needCountForWeapon; 
                var extra = evt.Amount - needCountForWeapon;
                weaponAmmo.ExtraCount += Mathf.Max(0, extra); 
            }
            else
            {
                weaponAmmo.Count += evt.Amount; 
            }     

            //update weapon event (add component to weapon)
            if (hasWeapon.CurrentUsedWeaponType == evt.Type)
                world.GetPool<WeaponChangeStateComponentTag>().Add(weaponEntity);
        }


        private ref HasWeaponComponent GetHasWeaponComponent(EcsWorld world, int heroEntity)
        {
            ref var hasWeapon = ref world.GetOrAddComponent<HasWeaponComponent>(heroEntity);
            hasWeapon.MyWeaponCollections ??= new Dictionary<WeaponType, int>();
            return ref hasWeapon;
        }


        private void CreateWeaponEntity(EcsWorld world, WeaponView weaponView, int ownerEntity, 
        ref HasWeaponComponent hasWeapon, ref CharacterPickupWeaponEvent pickupEvent)
        {
            var weaponEnt = world.NewEntity();

            //weapon
            ref var weapon = ref world.GetPool<WeaponComponent>().Add(weaponEnt);
            weapon.View = weaponView;
            
            if (IsRangeWeapon(pickupEvent))
            {
                //ammo
                ref var ammunition = ref world.GetPool<AmmunitionComponent>().Add(weaponEnt);
                ammunition.Count = weaponView.Settings.StartAmmo;
                ammunition.MaxAmmo = weaponView.Settings.StartAmmo;
                ammunition.ExtraCount = weaponView.Settings.MagAmountAmmo - weaponView.Settings.StartAmmo;
            }
            
            if (weapon.View.Settings.IsUseMeleeAttack) // melee attack weapon
            {
                ref var meleeAttack = ref world.GetPool<WeaponMeleeAttackComponent>().Add(weaponEnt);
                meleeAttack.OverlapResults = new Collider[32];
            }

            //owner
            ref var weaponOwner = ref world.GetPool<WeaponOwnerComponent>().Add(weaponEnt);
            weaponOwner.MyOwnerEntity = ownerEntity;

            //add to inventory
            hasWeapon.MyWeaponCollections.Add(pickupEvent.Type, weaponEnt);

            //Update ui
            world.GetPool<WeaponChangeStateComponentTag>().Add(weaponEnt);
        }


        private bool IsRangeWeapon(CharacterPickupWeaponEvent pickupEvent)
        {
            return pickupEvent.Type != WeaponType.Knife;
        }
    }
}