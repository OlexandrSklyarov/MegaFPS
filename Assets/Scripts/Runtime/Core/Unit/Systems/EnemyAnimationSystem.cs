using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _movementAnimFilter;
        private EcsFilter _attackAnimFilter;
        private EcsFilter _damageAnimFilter;
        private EcsFilter _deathAnimFilter;
        private EcsPool<EnemyAnimationComponent> _animationPool;
        private EcsPool<DeathAnimationEvent> _deathAnimEventPool;
        private EcsPool<NavigationComponent> _navPool;
        private EcsPool<EnemyAttackComponent> _attackPool;
        private EcsPool<UnitViewComponent> _viewPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _movementAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Inc<UnitViewComponent>()
                .Inc<NavigationComponent>()
                .Exc<EnemyAttackComponent>()
                .Exc<EnemyDamageEvent>()
                .Exc<DeathAnimationEvent>()
                .End();
                 
            _attackAnimFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAnimationComponent>()
                .Inc<EnemyAttackComponent>()
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
            _navPool = _world.GetPool<NavigationComponent>();
            _attackPool = _world.GetPool<EnemyAttackComponent>();
            _viewPool = _world.GetPool<UnitViewComponent>();
        }


        public void Run(IEcsSystems systems)
        {
            MovementHandle();
            DamageHandle();
            DeathHandle();
            AttackHandle();
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


        private void MovementHandle()
        {
            foreach (var ent in _movementAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);
                ref var navigation = ref _navPool.Get(ent);

                var normSpeed = navigation.AgentRef.speed / navigation.MaxSpeed;
                normSpeed = (navigation.AgentRef.enabled) ? normSpeed : 0f;
                animation.AnimatorRef.SetFloat(animation.SpeedPrm, normSpeed, 0.25f, Time.deltaTime);
            }
        }


        private void AttackHandle()
        {
            foreach (var ent in _attackAnimFilter)
            {
                ref var animation = ref _animationPool.Get(ent);
                ref var attack = ref _attackPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);

                if (!attack.IsAttackStarted)
                {
                    attack.IsAttackStarted = true;
                    attack.AttackCooldown = view.ViewRef.Config.AttackTimeCollection[attack.AttackIndex];

                    animation.AnimatorRef.SetInteger(animation.AttackIndexPrm, attack.AttackIndex);
                    animation.AnimatorRef.SetTrigger(animation.AttackPrm);
                }

            }
        }
    }
}