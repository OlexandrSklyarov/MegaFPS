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
                var vel = engine.RB.velocity;
                vel.y = 0f;

                var normSpeed = 0f;
                var hor = 0f;
                var vert = 0f;

                if (engine.IsGrounded && vel.sqrMagnitude > 0f) 
                {
                    normSpeed = (vel.sqrMagnitude > config.Prm.WalkSpeed) ? 1f : 0.5f;
                    vert = Vector3.Dot(engine.RB.velocity, engine.RB.transform.forward);
                    hor = Vector3.Dot(engine.RB.velocity, engine.RB.transform.right);
                }
                
                animation.HeadAnimatorRef.SetFloat(animation.SpeedPrm, normSpeed, 0.2f, Time.deltaTime);   
                animation.BodyAnimatorRef.SetFloat(animation.VertPrm, vert);   
                animation.BodyAnimatorRef.SetFloat(animation.HorPrm, hor);   
            }
        }      
    }
}