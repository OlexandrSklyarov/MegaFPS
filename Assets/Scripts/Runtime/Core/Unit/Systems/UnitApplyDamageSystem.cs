using UnityEngine;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class UnitApplyDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _damageEventFilter;
        private EcsFilter _raycastDamageFilter;
        private EcsFilter _overlapDamageFilter;
        private EcsPool<EnemyDamageEvent> _enemyDamageEvtPool;
        private EcsPool<RaycastDamageEvent> _raycastEvtPool;
        private EcsPool<OverlapDamageEvent> _overlapEvtPool;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<DeathComponent> _deathPool;
        private EcsPool<DeathAnimationEvent> _deathAnimEventPool;
        private EcsPool<PushRagdollEvent> _pushEventPool;
        private EcsPool<RagdollComponent> _ragdollPool;
        private EcsPool<UnitViewComponent> _viewPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _damageEventFilter = _world
                .Filter<EnemyDamageEvent>()
                .End();

            _raycastDamageFilter = _world
                .Filter<RaycastDamageEvent>()
                .Inc<UnitViewComponent>()
                .End();

            _overlapDamageFilter = _world
                .Filter<OverlapDamageEvent>()
                .Inc<UnitViewComponent>()
                .End();

            _enemyDamageEvtPool = _world.GetPool<EnemyDamageEvent>();
            _raycastEvtPool = _world.GetPool<RaycastDamageEvent>();
            _overlapEvtPool = _world.GetPool<OverlapDamageEvent>();
            _healthPool = _world.GetPool<HealthComponent>();
            _deathPool = _world.GetPool<DeathComponent>();
            _deathAnimEventPool = _world.GetPool<DeathAnimationEvent>();
            _pushEventPool = _world.GetPool<PushRagdollEvent>();
            _ragdollPool = _world.GetPool<RagdollComponent>();
            _viewPool = _world.GetPool<UnitViewComponent>();
        }


        public void Run(IEcsSystems systems)
        {  
            ClearDamageEvents();

            RaycastDamage();
         
            OverlapDamage();
        }


        private void ClearDamageEvents()
        {
            foreach (var ent in _damageEventFilter)
            {
                _enemyDamageEvtPool.Del(ent);
            }
        }


        private void OverlapDamage()
        {
            foreach (var ent in _overlapDamageFilter)
            {
                ref var evt = ref _overlapEvtPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);

                var isDeath = ChangeHP(ent, evt.Damage);                

                if (isDeath)
                {
                    var hitDir = view.ViewRef.transform.position - evt.DamageSource.position;
                    hitDir.y = 0f;

                    AddDeathAnimEvent(ent, ref view, hitDir);

                    if (evt.IsApplyPushForce)
                    {
                        TryActiveRagdoll(ent, hitDir.normalized, evt.DamageSource.position, evt.Power);                    
                    }
                }

                _overlapEvtPool.Del(ent);
            }
        }


        private void RaycastDamage()
        {
            foreach (var ent in _raycastDamageFilter)
            {
                ref var evt = ref _raycastEvtPool.Get(ent);
                ref var view = ref _viewPool.Get(ent);

                var isDeath = ChangeHP(ent, evt.Damage);

                if (isDeath)
                {
                    var hitDir = -evt.Hit.normal;

                    AddDeathAnimEvent(ent, ref view, hitDir);
                    
                    if (evt.IsApplyPushForce)
                    {
                        TryActiveRagdoll(ent, hitDir, evt.Hit.point, evt.Power);
                    }
                }

                _raycastEvtPool.Del(ent);
            }
        }


        private void TryActiveRagdoll(int ent, Vector3 hitDirection, Vector3 hitPoint, float power)
        {
            if (!_ragdollPool.Has(ent)) return;
            
            ref var pushEvt = ref _pushEventPool.Add(ent);
            pushEvt.HitDirection = hitDirection;
            pushEvt.HitPoint = hitPoint;
            pushEvt.Power = power;            
        }


        private bool ChangeHP(int ent, int damage)
        {
            var isDeath = false;

            if (_healthPool.Has(ent))
            {
                ref var hp = ref _healthPool.Get(ent);
                hp.Value -= damage;

                if (hp.Value <= 0)
                {
                    _healthPool.Del(ent);

                    //death component
                    ref var death = ref _deathPool.Add(ent);
                    death.PrepareDeathTime = 4f;                    

                    isDeath = true;
                }
                else
                {
                    _world.GetPool<EnemyDamageEvent>().Add(ent);
                }
            }

            return isDeath;
        }


        private void AddDeathAnimEvent(int ent, ref UnitViewComponent view, Vector3 hitDir)
        {
            var isForwardDie = Vector3.Dot(view.ViewRef.transform.forward, hitDir) > 0f;

            ref var deathEvent = ref _deathAnimEventPool.Add(ent);
            deathEvent.IsForwardDie = isForwardDie;
        }
    }
}