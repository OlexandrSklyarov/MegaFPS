using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroTag>()
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

                var speed = (input.IsRun) ? 
                    config.Prm.WalkSpeed * config.Prm.RunMultiplier : 
                    config.Prm.WalkSpeed;

                var velocity = new Vector3(input.Horizontal, 0f, input.Vertical);
                velocity = velocity.normalized * speed * Time.deltaTime;
                velocity = engine.CharacterController.transform.rotation * velocity;

                engine.CurrentMovement.x = velocity.x;
                engine.CurrentMovement.z = velocity.z;
                
                engine.CharacterController.Move(engine.CurrentMovement * Time.deltaTime);
            }
        }
    }
}