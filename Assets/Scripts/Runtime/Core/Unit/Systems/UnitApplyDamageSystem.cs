using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

namespace SA.FPS
{
    public sealed class UnitApplyDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _raycastDamageFilter;
        private EcsFilter _overlapDamageFilter;
        private EcsPool<RaycastDamageEvent> _raycastEvtPool;
        private EcsPool<OverlapDamageEvent> _overlapEvtPool;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<DeathComponent> _deathPool;
        private EcsPool<PushRagdollEvent> _pushEventPool;
        private EcsPool<RagdollComponent> _ragdollPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _raycastDamageFilter = _world
                .Filter<RaycastDamageEvent>()
                .End();

            _overlapDamageFilter = _world
                .Filter<OverlapDamageEvent>()
                .End();

            _raycastEvtPool = _world.GetPool<RaycastDamageEvent>();
            _overlapEvtPool = _world.GetPool<OverlapDamageEvent>();
            _healthPool = _world.GetPool<HealthComponent>();
            _deathPool = _world.GetPool<DeathComponent>();
            _pushEventPool = _world.GetPool<PushRagdollEvent>();
            _ragdollPool = _world.GetPool<RagdollComponent>();
        }


        public void Run(IEcsSystems systems)
        {  
            RaycastDamage();
         
            OverlapDamage();
        }


        private void OverlapDamage()
        {
            foreach (var ent in _overlapDamageFilter)
            {
                ref var evt = ref _overlapEvtPool.Get(ent);

                int totalDamage = (evt.DamageMultiplier > 0) ? evt.Damage * evt.DamageMultiplier : evt.Damage;
                ChangeHP(ent, totalDamage);

                _overlapEvtPool.Del(ent);
            }
        }


        private void RaycastDamage()
        {
            foreach (var ent in _raycastDamageFilter)
            {
                ref var evt = ref _raycastEvtPool.Get(ent);
                
                int totalDamage = (evt.DamageMultiplier > 0) ? evt.Damage * evt.DamageMultiplier : evt.Damage;
                var isDeath = ChangeHP(ent, totalDamage);

                if (isDeath && _ragdollPool.Has(ent))
                {
                   ref var pushEvt = ref _pushEventPool.Add(ent);
                   pushEvt.Hit = evt.Hit;
                   pushEvt.Power = evt.Power;
                }

                _raycastEvtPool.Del(ent);
            }
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
            }

            return isDeath;
        }
    }
}