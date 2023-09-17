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

                var horRotation = input.MouseX * _gameConfig.Control.MouseSensitivity;
                look.Body.Rotate(0f, horRotation, 0f);

                look.VerticalRotation -= input.MouseY * _gameConfig.Control.MouseSensitivity;
                look.VerticalRotation = Mathf.Clamp
                (
                    look.VerticalRotation, 
                    -_gameConfig.Control.UpDownAngle, 
                    _gameConfig.Control.UpDownAngle
                );

                look.FPS_Camera.transform.localRotation = Quaternion.Euler(look.VerticalRotation, 0f, 0f);
            }
        }
    }
}