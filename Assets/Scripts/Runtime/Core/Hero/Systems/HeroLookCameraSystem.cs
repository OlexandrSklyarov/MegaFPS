using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroLookCameraSystem : IEcsInitSystem, IEcsRunSystem
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
                ref var config = ref _configPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);

                CalculateVerticalRotation(ref look, ref input, ref config);
                CalculateHorizontalRotation(ref look, ref input, ref config);                
            }
        }


        private void CalculateHorizontalRotation(ref HeroLookComponent look, ref InputComponent input, ref CharacterConfigComponent config)
        {
            look.HorizontalRotation = input.MouseX * config.Prm.MouseSensitivity_X * Time.deltaTime;
            look.Body.Rotate(Vector3.up * look.HorizontalRotation);
        }


        private void CalculateVerticalRotation(ref HeroLookComponent look, ref InputComponent input, ref CharacterConfigComponent config)
        {
            look.VerticalRotation += -input.MouseY * config.Prm.MouseSensitivity_Y * Time.deltaTime;  
            look.VerticalRotation = Mathf.Clamp(look.VerticalRotation, config.Prm.DownAngle, config.Prm.UpAngle);

            look.HeadRoot.localRotation = Quaternion.Euler(look.VerticalRotation, 0f, 0f);
        }
    }
}