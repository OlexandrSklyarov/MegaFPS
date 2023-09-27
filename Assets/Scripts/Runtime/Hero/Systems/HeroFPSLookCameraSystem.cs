using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFPSLookCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameConfig _gameConfig;
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _gameConfig = systems.GetShared<SharedData>().Config;

            _filter = systems.GetWorld()
                .Filter<HeroTag>()
                .Inc<CharacterLookComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputComponent>()
                .Exc<TPSCameraTag>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var lookPool = world.GetPool<CharacterLookComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var look = ref lookPool.Get(ent);
                ref var input = ref inputPool.Get(ent);
                ref var config = ref configPool.Get(ent);

                HorizontalRotation(ref look, ref input);

                VerticalRotation(ref look, ref input);
            }
        }

        private void HorizontalRotation(ref CharacterLookComponent look, ref InputComponent input)
        {
            var targetHorizontalRotation = input.MouseX * _gameConfig.Control.MouseSensitivity;
            var curRot = look.Body.rotation;
            var newRot = curRot * Quaternion.Euler(0f, targetHorizontalRotation, 0f);            

            look.Body.rotation = Quaternion.Slerp(curRot, newRot, Time.deltaTime * _gameConfig.Control.SmoothHorizontalRotationSpeed);
        }

        private void VerticalRotation(ref CharacterLookComponent look, ref InputComponent input)
        {
            var newVertRotation = look.VerticalRotation;
            newVertRotation -= input.MouseY * _gameConfig.Control.MouseSensitivity;
            newVertRotation = Mathf.Clamp
            (
                newVertRotation,
                -_gameConfig.Control.UpDownAngle,
                _gameConfig.Control.UpDownAngle
            );

            look.VerticalRotation = Mathf.SmoothDampAngle
            (
                look.VerticalRotation,
                newVertRotation,
                ref look.VerticalVelocity,
                _gameConfig.Control.SmoothRotationTime,
                _gameConfig.Control.SmoothVerticalRotationSpeed
            );

            look.FPS_Camera.transform.localRotation = Quaternion.Euler(look.VerticalRotation, 0f, 0f);
        }
    }
}