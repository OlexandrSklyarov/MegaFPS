using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class UnitApplyDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _raycastDamageFilter;
        private EcsFilter _overlapDamageFilter;
        private EcsPool<RaycastDamageEvent> _raycastEvtPool;
        private EcsPool<OverlapDamageEvent> _overlapEvtPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _raycastDamageFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<RaycastDamageEvent>()
                .End();

            _overlapDamageFilter = _world
                .Filter<EnemyUnitTag>()
                .Inc<OverlapDamageEvent>()
                .End();

            _raycastEvtPool = _world.GetPool<RaycastDamageEvent>();
            _overlapEvtPool = _world.GetPool<OverlapDamageEvent>();
        }


        public void Run(IEcsSystems systems)
        {        
            //Raycast    
            foreach(var ent in _raycastDamageFilter)
            {
                ref var evt = ref _raycastEvtPool.Get(ent);

                Util.DebugUtil.PrintColor($"TEST >>> raycast damage mult {evt.DamageMultiplier}", Color.red);

                _raycastEvtPool.Del(ent);
            }

            //Overlap
            foreach(var ent in _raycastDamageFilter)
            {
                ref var evt = ref _overlapEvtPool.Get(ent);

                Util.DebugUtil.PrintColor($"TEST >>> overlap damage mult {evt.DamageMultiplier}", Color.yellow);

                _overlapEvtPool.Del(ent);
            }
        }
    }
}