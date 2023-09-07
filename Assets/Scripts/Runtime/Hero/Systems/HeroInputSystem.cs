using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private InputService _inputService;
        private EcsFilter _filter;        

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().Services.GetService<InputService>();
            _inputService.Controls.Enable();

            _filter = systems.GetWorld()
                .Filter<HeroTag>()
                .Inc<InputComponent>()
                .End();
        }

        public void Destroy(IEcsSystems systems)
        {
            _inputService.Controls.Disable();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var input = ref inputPool.Get(ent);

                var movement = _inputService.Controls.Player.Movement.ReadValue<Vector2>();
                input.Vertical = movement.x;
                input.Horizontal = movement.y;
            }
        }
    }
}