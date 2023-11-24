using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class WeaponMeleeAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;
        private EcsPool<WeaponMeleeAttackEvent> _evtPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;
        private EcsPool<WeaponMeleeAttackComponent> _meleeAttackPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _weaponFilter = world
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<WeaponMeleeAttackComponent>()
                .Inc<WeaponMeleeAttackEvent>()
                .End();

            _evtPool = world.GetPool<WeaponMeleeAttackEvent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
            _meleeAttackPool = world.GetPool<WeaponMeleeAttackComponent>();
        }

        public void Run(IEcsSystems systems)
        {                
            foreach(var ent in _weaponFilter)
            {
                _evtPool.Del(ent); 

                ref var weapon = ref _weaponPool.Get(ent);
                ref var owner = ref _ownerPool.Get(ent);
                ref var meleeAttack = ref _meleeAttackPool.Get(ent);
                                                                            
                AttackAnimation(ref weapon);
                PerformMeleeAttack(ref weapon, ref meleeAttack);                                       
            }             
        }


        private void AttackAnimation(ref WeaponComponent weapon)
        {
            weapon.View.MeleeAttack(out float duration);
        }


        private void PerformMeleeAttack(ref WeaponComponent weapon, ref WeaponMeleeAttackComponent meleeAttack)
        {
            if (!TryFindTargets(ref weapon, ref meleeAttack)) return;

            TryAttack(ref weapon, ref meleeAttack);
            
        }        

        private bool TryFindTargets(ref WeaponComponent weapon, ref WeaponMeleeAttackComponent meleeAttack)
        {
            meleeAttack.OverlapResultCount = Physics.OverlapSphereNonAlloc
            (
                weapon.View.FirePoint.position,
                weapon.View.Settings.OverlapRadius,
                meleeAttack.OverlapResults,
                weapon.View.Settings.TargetLayerMask                  
            );

            return meleeAttack.OverlapResultCount > 0;
        }


        private void TryAttack(ref WeaponComponent weapon, ref WeaponMeleeAttackComponent meleeAttack)
        {
            for (int i = 0; i < meleeAttack.OverlapResultCount; i++)
            {
                if (!meleeAttack.OverlapResults[i].TryGetComponent(out IAttackVisitor target)) continue;

                Util.DebugUtil.Print($"find target {meleeAttack.OverlapResults[i].name}");

                if (weapon.View.Settings.IsConsiderObstacles)
                {
                    Debug.DrawLine
                    (
                        weapon.View.FirePoint.position,
                        meleeAttack.OverlapResults[i].transform.position,
                        Color.red,
                        1f
                    );
                    var hasObstacle = Physics.Linecast
                    (
                        weapon.View.FirePoint.position,
                        meleeAttack.OverlapResults[i].transform.position,
                        weapon.View.Settings.ObstacleLayerMask
                    );

                    if (hasObstacle) continue;
                }

                target.Visit(weapon.View);
            }
        }        
    }
}