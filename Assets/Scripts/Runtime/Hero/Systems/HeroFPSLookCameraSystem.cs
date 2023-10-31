using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFPSLookCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<HeroLookComponent> _lookPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
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

                look.HeadRoot.position = look.CameraOrigin.transform.position;

                var mouse_x = input.MouseX;
                var mouse_y = input.MouseY;

                look.VerticalRotation -= mouse_y * config.Prm.MouseSensitivity * Time.deltaTime;
                look.VerticalRotation = Mathf.Clamp(look.VerticalRotation,config.Prm.UpAngle, config.Prm.DownAngle);

                look.HeadRoot.localRotation = Quaternion.Euler(look.VerticalRotation, 0f, 0f);
                look.Body.Rotate(Vector3.up, mouse_x * config.Prm.MouseSensitivity * Time.deltaTime);
            }
        }
    }
}