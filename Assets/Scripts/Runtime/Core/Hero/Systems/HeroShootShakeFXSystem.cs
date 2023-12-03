using DG.Tweening;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class HeroShootShakeFXSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CameraShakeComponent> _shakePool;
        private EcsPool<FPSCameraTransformComponent> _cameraTransformPool;
        

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<FPSCameraTransformComponent>()
                .Inc<CameraShakeComponent>()
                .End();

            var world = systems.GetWorld();
            _shakePool = world.GetPool<CameraShakeComponent>();
            _cameraTransformPool = world.GetPool<FPSCameraTransformComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var shake = ref _shakePool.Get(ent);
                ref var cameraTransform = ref _cameraTransformPool.Get(ent);

                var tr = cameraTransform.Value;

                tr.DOShakePosition(shake.Duration, shake.Strength, shake.Vibrato, shake.Randomness, false, true, ShakeRandomnessMode.Harmonic)
                    .SetEase(Ease.InOutBounce)
                    .SetLink(tr.gameObject);

                tr.DOShakeRotation(shake.Duration, shake.Strength, shake.Vibrato, shake.Randomness, true, ShakeRandomnessMode.Harmonic)
                    .SetEase(Ease.InOutBounce)
                    .SetLink(tr.gameObject);                

                _shakePool.Del(ent);
            }
        }
    }
}