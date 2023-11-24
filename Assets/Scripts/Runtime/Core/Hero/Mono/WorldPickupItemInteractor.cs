using Leopotam.EcsLite;

namespace SA.FPS
{
    public class WorldPickupItemInteractor : IPickupVisitor
    {
        private readonly EcsPackedEntity _heroEntity;
        private readonly EcsWorld _world;

        public WorldPickupItemInteractor(EcsWorld world, EcsPackedEntity entity)
        {
            _heroEntity = entity;
            _world = world;
        }

        void IPickupVisitor.Visit(WeaponPickupItem weapon)
        {
            if (!_heroEntity.Unpack(_world, out int ent)) return;

            ref var evt = ref _world.GetPool<CharacterPickupWeaponEvent>().Add(ent);
            evt.Type = weapon.Type;
            evt.Amount = weapon.Amount;
        }
    }
}