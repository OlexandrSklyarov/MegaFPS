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
            var ccPool = world.GetPool<CharacterEngineComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var cc = ref ccPool.Get(ent);
                ref var input = ref inputPool.Get(ent);
                ref var config = ref configPool.Get(ent);

                var velocity = new Vector3
                (
                    input.Vertical * config.Prm.WalkSpeed * Time.deltaTime, 
                    0f, 
                    input.Horizontal * config.Prm.WalkSpeed * Time.deltaTime
                );

                velocity = cc.CharacterController.transform.rotation * velocity;
                cc.CharacterController.SimpleMove(velocity);
            }
        }
    }
}