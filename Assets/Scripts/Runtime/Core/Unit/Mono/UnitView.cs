using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class UnitView : MonoBehaviour, IPoolable<UnitView>
    {
        public RagdollController Ragdoll => _ragdoll;

        [SerializeField] private BaseHitBox[] _hitBoxes;
        [SerializeField] private RagdollController _ragdoll;

        private IObjectPool<UnitView> _pool;

        private void OnValidate() 
        {
            _hitBoxes = GetComponentsInChildren<BaseHitBox>();
            _ragdoll = GetComponent<RagdollController>();
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

        public void Reclaim() => _pool.Release(this);
    }
}