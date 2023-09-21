using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroAnimationSystem: IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _data = systems.GetShared<SharedData>();
            
            _filter = systems.GetWorld()
                .Filter<HeroTag>()
                .Inc<CharacterAnimationComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<InputComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var animationPool = world.GetPool<CharacterAnimationComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var enginePool = world.GetPool<CharacterEngineComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var animation = ref animationPool.Get(ent);
                ref var config = ref configPool.Get(ent);
                ref var engine = ref enginePool.Get(ent);
                ref var input = ref inputPool.Get(ent);   

                //movement
                var isGrounded = engine.CharacterController.isGrounded;
                var vel = engine.CharacterController.velocity;
                vel.y = 0f;

                var normSpeed = (isGrounded) ? Mathf.Clamp01(vel.sqrMagnitude) : 0f;
                var multiplier = (input.IsRun) ? config.Prm.RunMultiplier : 1;
                
                animation.AnimatorRef.SetFloat("SPEED", normSpeed, 0.1f, Time.deltaTime);   
                animation.AnimatorRef.SetFloat("SPEED_MULT", multiplier);   

                //fire
                if (input.IsFire)             
                    animation.AnimatorRef.SetTrigger("SHOOT");                
            }
        }      
    }
}