using System;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class CreateEnemySpawnerSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var ent = world.NewEntity();
            
            ref var spawner = ref world.GetPool<EnemySpawnerComponent>().Add(ent);
               
            spawner.EnemiesTypeCount = Enum.GetNames(typeof(UnitType)).Length;
        }
    }
}