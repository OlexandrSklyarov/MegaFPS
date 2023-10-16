using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroMovementSystem : IEcsInitSystem, IEcsRunSystem
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

                engine.Speed = (input.IsRun) ? 
                    config.Prm.WalkSpeed * config.Prm.RunMultiplier : 
                    config.Prm.WalkSpeed;

                var velocity = new Vector3(input.Horizontal, 0f, input.Vertical);
                velocity = velocity.normalized * engine.Speed * Time.deltaTime;
                velocity = engine.CharacterController.transform.rotation * velocity;

                engine.CurrentMovement.x = velocity.x;
                engine.CurrentMovement.z = velocity.z;
                
                engine.CharacterController.Move(engine.CurrentMovement * Time.deltaTime);
            }
        }
    }
}