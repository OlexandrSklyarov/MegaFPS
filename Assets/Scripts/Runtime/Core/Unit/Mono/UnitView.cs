using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class UnitView : MonoBehaviour, IPoolable<UnitView>
    {
        public RagdollController Ragdoll => _ragdoll;
        public Animator Animator => _animator;

        [SerializeField] private BaseHitBox[] _hitBoxes;
        [SerializeField] private RagdollController _ragdoll;
        [SerializeField] private Animator _animator;

        private IObjectPool<UnitView> _pool;

        private void OnValidate() 
        {
            _hitBoxes = GetComponentsInChildren<BaseHitBox>();
            _ragdoll = GetComponent<RagdollController>();
            _animator = GetComponent<Animator>();
        }

        public void Init(EcsWorld world, EcsPackedEntity ownerEntity)
        {
            Array.ForEach(_hitBoxes, h => h.Setup(world, ownerEntity));
            _ragdoll.Off();
        }

        void IPoolable<UnitView>.SetPool(IObjectPool<UnitView> pool)
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