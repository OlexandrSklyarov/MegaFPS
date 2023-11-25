using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
                
                weapon.View.MeleeAttack(out float attackTime);

                PerformMeleeAttack(ref weapon, ref meleeAttack, attackTime);                                       
            }             
        }        


        private void PerformMeleeAttack(ref WeaponComponent weapon, 
            ref WeaponMeleeAttackComponent meleeAttack, float attackTime)
        {
            if (!TryFindTargets(ref weapon, ref meleeAttack)) return;

            TryAttack(ref weapon, ref meleeAttack, attackTime);
            
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


        private void TryAttack(ref WeaponComponent weapon, ref WeaponMeleeAttackComponent meleeAttack, float attackTime)
        {
            for (int i = 0; i < meleeAttack.OverlapResultCount; i++)
            {
                if (!meleeAttack.OverlapResults[i].TryGetComponent(out IAttackVisitable target)) continue;        

                if (weapon.View.Settings.IsConsiderObstacles)
                {                    
                    var hasObstacle = Physics.Linecast
                    (
                        weapon.View.FirePoint.position,
                        meleeAttack.OverlapResults[i].transform.position,
                        weapon.View.Settings.ObstacleLayerMask
                    );

                    if (hasObstacle) continue;
                }

                ExecuteAttackWithDelayAsync(target, weapon.View, attackTime);
            }
        }   


        private async void ExecuteAttackWithDelayAsync(IAttackVisitable target, WeaponView weaponView, float attackTime)
        {          
            await UniTask.Delay(TimeSpan.FromSeconds(attackTime * 0.5f));

            target.Visit(weaponView);
        }   
    }
}