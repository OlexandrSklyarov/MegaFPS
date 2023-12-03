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

                BodyRotation(ref look, ref input, ref config);
                CameraRotation(ref look, ref input, ref config);   
            }
        }


        private void BodyRotation(ref HeroLookComponent look, ref InputComponent input, ref CharacterConfigComponent config)
        {
            look.HorizontalRotationAngle += input.MouseX * config.Prm.MouseSensitivity_X;   
            var newRot = look.OriginBodyRotation * Quaternion.AngleAxis(look.HorizontalRotationAngle, Vector3.up);

            look.Body.rotation = Quaternion.Slerp
            (
                look.Body.rotation,
                newRot,
                config.Prm.SmoothRotation * Time.deltaTime
            );
        }


        private void CameraRotation(ref HeroLookComponent look, ref InputComponent input, ref CharacterConfigComponent config)
        {
            look.VerticalRotationAngle += input.MouseY * config.Prm.MouseSensitivity_Y; 

            look.VerticalRotationAngle = Mathf.Clamp
            (
                look.VerticalRotationAngle, 
                config.Prm.DownAngle, 
                config.Prm.UpAngle
            );

            var newRot = look.OriginCameraRotation * Quaternion.AngleAxis(-look.VerticalRotationAngle, Vector3.right);
            
            look.HeadRoot.localRotation = Quaternion.Slerp
            (
                look.HeadRoot.localRotation,
                newRot,
                config.Prm.SmoothRotation * Time.deltaTime
            );
        }
    }
}