using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroCheckGroundSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _enginePool;
        private EcsPool<CharacterConfigComponent> _configPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<CharacterConfigComponent>()
                .End();

            var world = systems.GetWorld();
            _enginePool = world.GetPool<CharacterEngineComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
        }


        public void Run(IEcsSystems systems)
        {        
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                var origin = engine.RB.transform.position + Vector3.up * 0.3f;

                engine.IsGrounded = Physics.Raycast
                (
                    origin,
                    Vector3.down,
                    0.4f,
                    ~config.Prm.HeroLayer
                );
            }
        }
    }
}