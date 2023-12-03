using System;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsFilter _damageAnimFilter;
        private EcsPool<EnemyAnimationComponent> _animationPool;
        private EcsPool<EnemyDamageEvent> _damageEventPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .End(); 

            _damageAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Inc<EnemyDamageEvent>()
                .End();           

            _animationPool = _world.GetPool<EnemyAnimationComponent>();
            _damageEventPool = _world.GetPool<EnemyDamageEvent>();
        }


        public void Run(IEcsSystems systems)
        {
            AnimationHandle();
            DamageAnimationHandle();
        }


        private void DamageAnimationHandle()
        {
            foreach (var ent in _damageAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);
                animation.AnimatorRef.SetTrigger(animation.DamagePrm);
            }
        }


        private void AnimationHandle()
        {
            foreach (var ent in _filter)
            {
                ref var animation = ref _animationPool.Get(ent);

            }
        }
    }
}