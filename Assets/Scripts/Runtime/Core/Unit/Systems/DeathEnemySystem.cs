using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class DeathEnemySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<DeathComponent> _deathPool;
        private EcsPool<UnitViewComponent> _viewPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<DeathComponent>()
                .Inc<UnitViewComponent>()
                .End();           

            _deathPool = _world.GetPool<DeathComponent>();
            _viewPool = _world.GetPool<UnitViewComponent>();
        }


        public void Run(IEcsSystems systems)
        {  
            foreach(var ent in _filter)
            {
                ref var death = ref _deathPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);

                if (death.PrepareDeathTime > 0)
                {
                    death.PrepareDeathTime -= Time.deltaTime;
                    continue;
                }

                view.ViewRef.Reclaim();
                
                _world.DelEntity(ent);
            }
        }
    }
}