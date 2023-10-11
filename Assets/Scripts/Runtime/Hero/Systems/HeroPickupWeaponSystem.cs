using System.Linq;
using Leopotam.EcsLite;
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
            var weaponType = evt.Type;
            var weapon = hero.View.WeaponViews.First(x => x.Type == weaponType);
            weapon.Show();

            var ent = world.NewEntity();
            ref var newEvt = ref world.GetPool<TakeWeaponEvent>().Add(ent);
            newEvt.OwnerEntity = heroEntity;
            newEvt.WeaponView = weapon;        
            newEvt.StartAmmo = weapon.Settings.StartAmmo;        
        }
    }
}