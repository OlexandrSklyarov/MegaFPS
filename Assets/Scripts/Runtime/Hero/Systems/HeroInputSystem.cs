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
                .Filter<HeroComponent>()
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

                //direction
                var movement = _inputService.Controls.Player.Movement.ReadValue<Vector2>();
                input.Horizontal = movement.x;
                input.Vertical = movement.y;

                //rotation
                var rotation = _inputService.Controls.Player.Look.ReadValue<Vector2>();
                input.MouseX = rotation.x;
                input.MouseY = rotation.y;

                //run
                input.IsRun = _inputService.Controls.Player.Run.ReadValue<float>() > 0;

                //jump
                input.IsJump = _inputService.Controls.Player.Jump.ReadValue<float>() > 0;

                //fire
                input.IsFire = _inputService.Controls.Player.Fire.ReadValue<float>() > 0;

                //melle attack
                input.IsAttack = _inputService.Controls.Player.Attack.ReadValue<float>() > 0;

                //reload
                input.IsReload = _inputService.Controls.Player.Reload.ReadValue<float>() > 0;
            }
        }
    }
}