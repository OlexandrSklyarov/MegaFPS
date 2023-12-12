using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class EnemyUnitView : MonoBehaviour, IPoolable<EnemyUnitView>
    {
        public EnemyUnitConfig Config => _config;
        public RagdollController Ragdoll => _ragdoll;
        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navAgent;

        [SerializeField] private EnemyUnitConfig _config;
        [SerializeField] private BaseHitBox[] _hitBoxes;
        [SerializeField] private RagdollController _ragdoll;
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navAgent;

        private IObjectPool<EnemyUnitView> _pool;

        private void OnValidate() 
        {
            _hitBoxes = GetComponentsInChildren<BaseHitBox>();
            _ragdoll = GetComponent<RagdollController>();
            _animator = GetComponentInChildren<Animator>();
            _navAgent = GetComponentInChildren<NavMeshAgent>();
        }

        public void Init(EcsWorld world, EcsPackedEntity ownerEntity)
        {
            Array.ForEach(_hitBoxes, h => h.Setup(world, ownerEntity));
            _ragdoll.Off();
        }

        void IPoolable<EnemyUnitView>.SetPool(IObjectPool<EnemyUnitView> pool)
        {
            _pool = pool;
        }

        public void Reclaim() 
        {
            _ragdoll.Off();
            _pool.Release(this);
        } 
    }
}