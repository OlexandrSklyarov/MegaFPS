using System;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _defaultAnimFilter;
        private EcsFilter _damageAnimFilter;
        private EcsFilter _deathAnimFilter;
        private EcsPool<EnemyAnimationComponent> _animationPool;
        private EcsPool<DeathAnimationEvent> _deathAnimEventPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _defaultAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Exc<EnemyDamageEvent>()
                .Exc<DeathAnimationEvent>()
                .End(); 

            _damageAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Inc<EnemyDamageEvent>()
                .Exc<DeathAnimationEvent>()
                .End();  

            _deathAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Inc<DeathAnimationEvent>()
                .End();                      

            _animationPool = _world.GetPool<EnemyAnimationComponent>();
            _deathAnimEventPool = _world.GetPool<DeathAnimationEvent>();
        }


        public void Run(IEcsSystems systems)
        {
            AnimationHandle();
            DamageHandle();
            DeathHandle();
        }


        private void DeathHandle()
        {
            foreach (var ent in _deathAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);
                ref var evt = ref _deathAnimEventPool.Get(ent);
                var deathTriggerPrm = (evt.IsForwardDie) ? animation.DeathForwardPrm : animation.DeathBackwardPrm;
                animation.AnimatorRef.SetTrigger(deathTriggerPrm);
                
                _deathAnimEventPool.Del(ent);
            }
        }


        private void DamageHandle()
        {
            foreach (var ent in _damageAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);
                animation.AnimatorRef.SetTrigger(animation.DamagePrm);
            }
        }


        private void AnimationHandle()
        {
            foreach (var ent in _defaultAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);

            }
        }
    }
}