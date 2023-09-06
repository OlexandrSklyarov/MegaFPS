using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroSpawnSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<SharedData>();          

            var heroView = GetView(data.Config.HeroPrefab, data.WorldData.HeroSpawnPoint);
            var entity = world.NewEntity();

            //tag
            world.GetPool<HeroTag>().Add(entity);

            //cc
            ref var engine = ref world.GetPool<CharacterEngineComponent>().Add(entity);
            engine.CharacterController = heroView.CharacterController;

            //input
            world.GetPool<InputComponent>().Add(entity);
        }

        private HeroView GetView(HeroView heroPrefab, Transform heroSpawnPoint)
        {
            var hero = UnityEngine.Object.Instantiate
            (
                heroPrefab,
                heroSpawnPoint.position,
                heroSpawnPoint.rotation
            );

            hero.Init();

            return hero;
        }
    }
}