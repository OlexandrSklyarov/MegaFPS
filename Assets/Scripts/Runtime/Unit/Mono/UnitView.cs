using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class UnitView : MonoBehaviour, IPoolable<UnitView>
    {
        [SerializeField] private BaseHitBox[] _hitBoxes;

        private IObjectPool<UnitView> _pool;

        private void OnValidate() 
        {
            _hitBoxes = GetComponentsInChildren<BaseHitBox>();
        }

        public void Init(EcsWorld world, EcsPackedEntity ownerEntity)
        {
            Array.ForEach(_hitBoxes, h => h.Setup(world, ownerEntity));
        }

        void IPoolable<UnitView>.SetPool(IObjectPool<UnitView> pool)
        {
            _pool = pool;
        }

        public void Reclaim() => _pool.Release(this);
    }
}