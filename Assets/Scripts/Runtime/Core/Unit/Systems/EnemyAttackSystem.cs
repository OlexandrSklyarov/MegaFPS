using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class EnemyAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyAttackComponent> _attackPool;
        private EcsPool<UnitViewComponent> _viewPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<EnemyUnitTag>()
                .Inc<EnemyAttackComponent>()
                .Inc<UnitViewComponent>()
                .End();             

            _attackPool = _world.GetPool<EnemyAttackComponent>();
            _viewPool = _world.GetPool<UnitViewComponent>();
        }


        public void Run(IEcsSystems systems)
        {           
            foreach(var ent in _filter)
            {
                ref var attack = ref _attackPool.Get(ent); 
                ref var view = ref _viewPool.Get(ent); 
                
                RotateToTarget(ref attack, ref view);

                if (!attack.IsAttackStarted)
                {
                    continue;
                } 

                if (attack.AttackCooldown > 0f)
                {
                    attack.AttackCooldown -= Time.deltaTime;
                    continue;
                }               

                //reset Attack
                _attackPool.Del(ent); 
            }
        }

        private void RotateToTarget(ref EnemyAttackComponent attack, ref UnitViewComponent view)
        {
            var dir = (attack.TargetPos - view.ViewRef.transform.position).normalized;
            var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            view.ViewRef.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}