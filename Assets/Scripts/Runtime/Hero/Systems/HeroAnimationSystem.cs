using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroAnimationSystem: IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<CharacterAnimationComponent> _animationPool;
        private EcsPool<CharacterAttackComponent> _attackPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<CharacterEngineComponent> _enginePool;

        public void Init(IEcsSystems systems)
        {
            _data = systems.GetShared<SharedData>();
            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterAnimationComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<InputComponent>()
                .End();

            var world = systems.GetWorld();
            _animationPool = world.GetPool<CharacterAnimationComponent>();
            _attackPool = world.GetPool<CharacterAttackComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _enginePool = world.GetPool<CharacterEngineComponent>();
        }

        public void Run(IEcsSystems systems)
        {      
            foreach(var ent in _filter)
            {
                ref var animation = ref _animationPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var engine = ref _enginePool.Get(ent);
                ref var attack = ref _attackPool.Get(ent); 

                //movement
                var isGrounded = engine.CharacterController.isGrounded;
                var vel = engine.CharacterController.velocity;
                vel.y = 0f;

                var normSpeed = 0f;

                if (isGrounded && vel.sqrMagnitude > 0f) 
                {
                    normSpeed = (engine.Speed > config.Prm.WalkSpeed) ? 1f : 0.5f;
                }
                
                animation.AnimatorRef.SetFloat("SPEED", normSpeed, 0.2f, Time.deltaTime);   
            }
        }      
    }
}