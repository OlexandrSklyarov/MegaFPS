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

                var origin = engine.RB.transform.position + Vector3.up * config.Prm.GroundCheckDistance * 0.9f;

                var isGroundResult = Physics.SphereCast
                (
                    origin,
                    config.Prm.GroundCheckSphereCastRadius,
                    Vector3.down,
                    out var hit,
                    config.Prm.GroundCheckDistance,
                    ~config.Prm.HeroLayer
                );

                if (!engine.IsGrounded && isGroundResult) 
                    engine.NextJumpTime = Time.time + 0.1f;
                
                engine.IsGrounded = isGroundResult;
            }
        }
    }
}