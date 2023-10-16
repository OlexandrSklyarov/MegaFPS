using DG.Tweening;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class HeroShootShakeFXSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CameraShakeComponent> _shakePool;
        private EcsPool<CharacterLookComponent> _lookPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterLookComponent>()
                .Inc<CameraShakeComponent>()
                .End();

            var world = systems.GetWorld();
            _shakePool = world.GetPool<CameraShakeComponent>();
            _lookPool = world.GetPool<CharacterLookComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var shake = ref _shakePool.Get(ent);
                ref var look = ref _lookPool.Get(ent);

                var tr = look.FPS_Camera.transform;

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