using DG.Tweening;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class HeroShootShakeFXSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<CharacterLookComponent>()
                .Inc<CameraShakeComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var shakePool = world.GetPool<CameraShakeComponent>();
            var lookPool = world.GetPool<CharacterLookComponent>();

            foreach(var ent in _filter)
            {
                ref var shake = ref shakePool.Get(ent);
                ref var look = ref lookPool.Get(ent);

                var tr = look.FPS_Camera.transform;

                tr.DOShakePosition(shake.Duration, shake.Strength, shake.Vibrato, shake.Randomness, false, true, ShakeRandomnessMode.Harmonic)
                    .SetEase(Ease.InOutBounce)
                    .SetLink(tr.gameObject);

                tr.DOShakeRotation(shake.Duration, shake.Strength, shake.Vibrato, shake.Randomness, true, ShakeRandomnessMode.Harmonic)
                    .SetEase(Ease.InOutBounce)
                    .SetLink(tr.gameObject);                

                shakePool.Del(ent);
            }
        }
    }
}