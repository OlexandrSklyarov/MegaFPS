using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroJumpingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputComponent>()
                .End();
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var enginePool = world.GetPool<CharacterEngineComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var engine = ref enginePool.Get(ent);
                ref var input = ref inputPool.Get(ent);
                ref var config = ref configPool.Get(ent);

                if (engine.CharacterController.isGrounded)
                {
                    engine.CurrentMovement.y = -0.5f;

                    if (input.IsJump)
                    {
                        engine.CurrentMovement.y = config.Prm.JumpForce;
                    }
                }
                else
                {
                    engine.CurrentMovement.y -= config.Prm.Gravity * Time.deltaTime;
                }
            }
        }
    }
}