using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class UnitSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _enemiesFilter;
        private EcsPool<EnemySpawnerComponent> _spawnerPool;
        private WorldData _data;
        private IPoolManager _poolManager;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _data = systems.GetShared<SharedData>().WorldData;
            _poolManager = ServicesPool.Instance.GetService<IPoolManager>();

            _world = systems.GetWorld();

            _filter = _world
                .Filter<EnemySpawnerComponent>()
                .End();

            _enemiesFilter = _world
                .Filter<EnemyUnitTag>()
                .End();

            _spawnerPool = _world.GetPool<EnemySpawnerComponent>();
        }


        public void Run(IEcsSystems systems)
        {            
            foreach(var ent in _filter)
            {
                ref var spawner = ref _spawnerPool.Get(ent);

                if (spawner.NextCheckSpawnTime > Time.time) continue;
                if (_enemiesFilter.GetEntitiesCount() >= 2) continue;

                var unitType = GetRandomType(ref spawner);
                SpawnUnit(unitType);
            }
        }


        private void SpawnUnit(UnitType unitType)
        {
            var ent = _world.NewEntity();

            var enemyView = _poolManager.GetUnitView(unitType);
            enemyView.transform.position = GetRandomPoint().position;
            enemyView.Init(_world, _world.PackEntity(ent));

            ref var enemy = ref _world.GetPool<EnemyUnitTag>().Add(ent); 

            //health
            ref var hp = ref _world.GetPool<HealthComponent>().Add(ent);  
            hp.Value = 100;          
            
            //ragdoll
            ref var ragdoll = ref _world.GetPool<RagdollComponent>().Add(ent);  
            ragdoll.Controller = enemyView.Ragdoll;          
        }


        private UnitType GetRandomType(ref EnemySpawnerComponent spawner)
        {            
            return (UnitType)UnityEngine.Random.Range(0, spawner.EnemiesTypeCount);
        }


        private Transform GetRandomPoint() 
        {
            return _data.EnemySpawnPoints[UnityEngine.Random.Range(0, _data.EnemySpawnPoints.Length)];
        }
    }
}