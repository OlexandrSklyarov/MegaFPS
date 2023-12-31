using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private InputService _inputService;
        private EcsFilter _filter;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _inputService = ServicesPool.Instance.GetService<InputService>();
            _inputService.Controls.Enable();

            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<InputComponent>()
                .End();

            var world = systems.GetWorld();
            _inputPool = world.GetPool<InputComponent>();
        }

        public void Destroy(IEcsSystems systems)
        {
            _inputService.Controls.Disable();
        }

        public void Run(IEcsSystems systems)
        {     
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);

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

                if (input.IsFire || input.IsAttack)
                {
                    world.GetOrAddComponent<CharacterAttackEvent>(ent);
                }

                //reload
                if (_inputService.Controls.Player.Reload.ReadValue<float>() > 0)
                {                    
                    world.GetOrAddComponent<CharacterReloadWeaponEvent>(ent);
                }

                //switch weapon
                if (_inputService.Controls.Player.SwitchWeapon.ReadValue<float>() > 0)
                {
                    world.GetOrAddComponent<SwitchWeaponEvent>(ent);
                }
            }
        }
    }
}