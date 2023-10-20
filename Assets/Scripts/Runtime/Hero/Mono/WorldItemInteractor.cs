using Leopotam.EcsLite;
using Runtime.Services.WeaponsFactory;

namespace SA.FPS
{
    public class WorldItemInteractor : IPickupVisitor
    {
        private readonly EcsPackedEntity _heroEntity;
        private readonly EcsWorld _world;

        public WorldItemInteractor(EcsWorld world, EcsPackedEntity entity)
        {
            _heroEntity = entity;
            _world = world;
        }

        void IPickupVisitor.PickupWeapon(WeaponType type, int amount)
        {
            if (!_heroEntity.Unpack(_world, out int ent)) return;

            ref var evt = ref _world.GetPool<CharacterPickupWeaponEvent>().Add(ent);
            evt.Type = type;
            evt.Amount = amount;
        }
    }
}