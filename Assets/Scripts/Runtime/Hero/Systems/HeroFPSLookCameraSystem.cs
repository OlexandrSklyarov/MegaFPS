using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFPSLookCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<HeroLookComponent> _lookPool;
        private EcsPool<CharacterConfigComponent> _configPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HeroLookComponent>()
                .Inc<CharacterConfigComponent>()
                .Exc<TPSCameraTag>()
                .End();

            var world = systems.GetWorld();
            _lookPool = world.GetPool<HeroLookComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var look = ref _lookPool.Get(ent);
                ref var config = ref _configPool.Get(ent);

                look.VirtualCameraAimPOV.m_VerticalAxis.m_MinValue = config.Prm.DownAngle;
                look.VirtualCameraAimPOV.m_VerticalAxis.m_MaxValue = config.Prm.UpAngle;
                look.VirtualCameraAimPOV.m_VerticalAxis.m_MaxSpeed = config.Prm.MouseSensitivity_Y;
                look.VirtualCameraAimPOV.m_HorizontalAxis.m_MaxSpeed = config.Prm.MouseSensitivity_X;
                
                look.HeadRoot.localRotation = Quaternion.Euler(look.FPS_VirtualCamera.eulerAngles.x, 0f, 0f);
                look.Body.rotation = Quaternion.Euler(0f, look.FPS_VirtualCamera.eulerAngles.y, 0f);
            }
        }
    }
}