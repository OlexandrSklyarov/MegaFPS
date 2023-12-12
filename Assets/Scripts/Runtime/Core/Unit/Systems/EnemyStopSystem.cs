using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class EnemyStopSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _enemyDamageFilter;
        private EcsFilter _enemyDeathFilter;
        private EcsPool<NavigationComponent> _navPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _enemyDamageFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<NavigationComponent>()
                .Inc<EnemyDamageEvent>()
                .End();

            _enemyDeathFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<NavigationComponent>()
                .Inc<DeathComponent>()
                .End();

            _navPool = _world.GetPool<NavigationComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _enemyDamageFilter)
            {
                ref var navigation = ref _navPool.Get(ent);
                navigation.StopDelay = 2f;
                navigation.AgentRef.enabled = false;
            }  

            foreach (var ent in _enemyDeathFilter)
            {
                ref var navigation = ref _navPool.Get(ent);
                navigation.AgentRef.enabled = false;
                _navPool.Del(ent);                
            }           
        }
    }
}