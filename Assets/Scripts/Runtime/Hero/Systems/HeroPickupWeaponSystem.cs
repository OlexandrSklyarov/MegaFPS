using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using Util;

namespace SA.FPS
{
    public sealed class HeroPickupWeaponSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IWeaponItemFactory _weaponFactory;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _weaponFactory = systems.GetShared<SharedData>().Services.GetService<IWeaponItemFactory>();

            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<InventoryComponent>()
                .Inc<CharacterPickupWeaponEvent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var heroPool = world.GetPool<HeroComponent>();
            var inventoryPool = world.GetPool<InventoryComponent>();
            var evtPool = world.GetPool<CharacterPickupWeaponEvent>();

            foreach(var ent in _filter)
            {
                ref var hero = ref heroPool.Get(ent);
                ref var inventory = ref inventoryPool.Get(ent);
                ref var evt = ref evtPool.Get(ent);

                var weapon = _weaponFactory.CreateWeaponItem(evt.Type);
                weapon.State.Amount = evt.Amount;

                if (inventory.InventoryRef.TryAdd(this, weapon))
                {
                    DebugUtility.Print($"Pickup weapon {evt.Type}");
                    HeroTakeWeaponEvent(world, ent, ref evt, ref hero);
                }

                evtPool.Del(ent);
            }
        }


        private void HeroTakeWeaponEvent(EcsWorld world, int heroEntity, ref CharacterPickupWeaponEvent evt, ref HeroComponent hero)
        {
            var pickupWeaponType = evt.Type;  

            ref var hasWeapon = ref GetHasWeaponComponent(world, heroEntity);       
            
            //weapon not found
            if (!IsHasWeaponInInventory(pickupWeaponType, ref hasWeapon))
            {
                //try hide previous weapon
                var curWeaponType = hasWeapon.CurrentWeaponType;
                var perviousWeaponView = hero.View.WeaponViews.First(x => x.Type == curWeaponType);
                if (perviousWeaponView != null) perviousWeaponView.Hide();

                //try show new weapon
                var weaponView = hero.View.WeaponViews.First(x => x.Type == pickupWeaponType); 
                weaponView.Show();            

                var newWeaponEntity = CreateWeaponEntity(world, weaponView, heroEntity);  

                //save weapon
                hasWeapon.CurrentWeaponType = pickupWeaponType;
                hasWeapon.MyWeapons.Add(pickupWeaponType, newWeaponEntity);
            }
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