using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFPSLookCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameConfig _gameConfig;
        private EcsFilter _filter;
        private EcsPool<HeroLookComponent> _lookPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _gameConfig = systems.GetShared<SharedData>().Config;

            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HeroLookComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputComponent>()
                .Exc<TPSCameraTag>()
                .End();

            var world = systems.GetWorld();
            _lookPool = world.GetPool<HeroLookComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _inputPool = world.GetPool<InputComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var look = ref _lookPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                HorizontalRotation(ref look, ref input);

                VerticalRotation(ref look, ref input);
            }
        }


        private void HorizontalRotation(ref HeroLookComponent look, ref InputComponent input)
        {
            var targetHorizontalRotation = input.MouseX * _gameConfig.Control.MouseSensitivity;
            var curRot = look.Body.rotation;
            var newRot = curRot * Quaternion.Euler(0f, targetHorizontalRotation, 0f);            

            look.Body.rotation = Quaternion.Slerp(curRot, newRot, Time.deltaTime * _gameConfig.Control.SmoothHorizontalRotation);
        }


        private void VerticalRotation(ref HeroLookComponent look, ref InputComponent input)
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
                _gameConfig.Control.SmoothVerticalRotation
            );

            //rotate Head
            look.HeadRoot.transform.localRotation = Quaternion.Euler(look.VerticalRotation, 0f, 0f);
        }
    }
}