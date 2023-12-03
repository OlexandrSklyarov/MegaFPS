using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

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

                if (spawner.NextCheckSpawnTime > 0f) 
                {
                    spawner.NextCheckSpawnTime -= Time.deltaTime;
                    continue;
                }

                if (_enemiesFilter.GetEntitiesCount() >= 2) continue;

                var unitType = GetRandomType(ref spawner);
                var spawnPosition = GetRandomPosition(ref spawner);

                SpawnUnit(unitType, spawnPosition);

                spawner.NextCheckSpawnTime = 2f;
            }
        }


        private void SpawnUnit(UnitType unitType, Vector3 spawnPosition)
        {
            var ent = _world.NewEntity();

            var enemyView = _poolManager.GetUnitView(unitType);
            enemyView.transform.position = spawnPosition;
            enemyView.Init(_world, _world.PackEntity(ent));

            ref var enemy = ref _world.GetPool<EnemyUnitTag>().Add(ent); 

            //View
            ref var v = ref _world.GetPool<UnitViewComponent>().Add(ent);  
            v.ViewRef = enemyView; 
            
            //health
            ref var hp = ref _world.GetPool<HealthComponent>().Add(ent);  
            hp.Value = 100;          
            
            //ragdoll
            ref var ragdoll = ref _world.GetPool<RagdollComponent>().Add(ent);  
            ragdoll.Controller = enemyView.Ragdoll;   
           
            //animation
            ref var animation = ref _world.GetPool<EnemyAnimationComponent>().Add(ent);  
            animation.AnimatorRef = enemyView.Animator; 
            animation.DamagePrm = Animator.StringToHash("Damage");     
            animation.SpeedPrm = Animator.StringToHash("Speed");     
            animation.AttackPrm = Animator.StringToHash("Attack");     
            animation.AttackIndexPrm = Animator.StringToHash("Attack_index");     
        }


        private UnitType GetRandomType(ref EnemySpawnerComponent spawner)
        {            
            return (UnitType)UnityEngine.Random.Range(0, spawner.EnemiesTypeCount);
        }


        private Vector3 GetRandomPosition(ref EnemySpawnerComponent spawner) 
        {
            if (spawner.RandomPoints == null || spawner.RandomPoints.Count <= 0)
            {
                var list = new List<int>();

                for (int i = 0; i < _data.EnemySpawnPoints.Length; i++)
                {
                    list.Add(i);
                }

                list.Shuffle();

                spawner.RandomPoints = new Queue<int>(list);
            }

            var nextIndex = spawner.RandomPoints.Dequeue();
            
            return _data.EnemySpawnPoints[nextIndex].position;
        }
    }
}