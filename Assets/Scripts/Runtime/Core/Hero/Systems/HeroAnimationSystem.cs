using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroAnimationSystem: IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterAnimationComponent> _animationPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<CharacterEngineComponent> _enginePool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterAnimationComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<InputComponent>()
                .End();

            var world = systems.GetWorld();
            _animationPool = world.GetPool<CharacterAnimationComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _enginePool = world.GetPool<CharacterEngineComponent>(); 
            _inputPool = world.GetPool<InputComponent>(); 
        }

        public void Run(IEcsSystems systems)
        {      
            foreach(var ent in _filter)
            {
                ref var animation = ref _animationPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var engine = ref _enginePool.Get(ent);
                ref var input = ref _inputPool.Get(ent); 

                //movement
                var isMoving = input.Vertical != 0f || input.Horizontal != 0f;
                var vel = engine.RB.velocity;
                vel.y = 0f;

                var normSpeed = 0f;

                if (engine.IsGrounded && isMoving) 
                {
                    normSpeed = (vel.sqrMagnitude > config.Prm.WalkSpeed * config.Prm.WalkSpeed) ? 1f : 0.5f;
                }
                
                animation.HeadAnimatorRef.SetFloat(animation.SpeedPrm, normSpeed, 0.1f, Time.deltaTime);  
            }
        }      
    }
}