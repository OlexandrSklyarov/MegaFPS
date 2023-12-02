using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyAnimationComponent> _animationPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .End();           

            _animationPool = _world.GetPool<EnemyAnimationComponent>();
        }


        public void Run(IEcsSystems systems)
        {  
            foreach(var ent in _filter)
            {
                ref var animation = ref _animationPool.Get(ent);             
                
            }
        }
    }
}