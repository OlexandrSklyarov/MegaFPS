using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroPhysicsMovementSystem : IEcsInitSystem, IEcsRunSystem
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

                var moveDirection = new Vector3(input.Horizontal, 0f, input.Vertical);

                var speed = (moveDirection == Vector3.zero) ? 0f : (input.IsRun) ? 
                    config.Prm.RunSpeed : config.Prm.WalkSpeed;  

                engine.CurrentSpeed = Mathf.SmoothDamp
                (
                    engine.CurrentSpeed,
                    speed,
                    ref engine.CurrentSmoothVelocity,
                    config.Prm.SmoothSpeedTime
                );           

                var move = engine.RB.transform.TransformDirection(moveDirection) * engine.CurrentSpeed;
                var nextPosition = engine.RB.transform.position + move * Time.fixedDeltaTime;
                engine.RB.MovePosition(nextPosition);
            }
        }
    }
}