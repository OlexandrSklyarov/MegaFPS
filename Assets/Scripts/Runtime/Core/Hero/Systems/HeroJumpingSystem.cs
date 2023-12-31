using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroJumpingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _enginePool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputComponent>()
                .End();

            var world = systems.GetWorld();
            _enginePool = world.GetPool<CharacterEngineComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _inputPool = world.GetPool<InputComponent>();
        }


        public void Run(IEcsSystems systems)
        {            
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                if (engine.IsGrounded && input.IsJump && Time.time > engine.NextJumpTime)
                {
                    engine.RB.AddForce(Vector3.up * config.Prm.JumpForce, ForceMode.VelocityChange); 
                    engine.NextJumpTime = Time.time + 0.5f;               
                }
            }
        }
    }
}