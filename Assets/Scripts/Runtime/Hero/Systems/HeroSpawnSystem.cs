using System;
using Cinemachine;
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
            
            //cc
            ref var engine = ref world.GetPool<CharacterEngineComponent>().Add(entity);
            engine.CharacterController = heroView.CharacterController;

            //tag
            world.GetPool<HeroTag>().Add(entity);

            //config
            ref var config = ref world.GetPool<CharacterConfigComponent>().Add(entity);
            config.Prm = heroView.Config;

            //config
            ref var camera = ref world.GetPool<CameraComponent>().Add(entity);
            camera.Virtual = GetCamera(data, heroView);

            //input
            world.GetPool<InputComponent>().Add(entity);
        }

        private CinemachineVirtualCamera GetCamera(SharedData data, HeroView heroView)
        {
            var camera = UnityEngine.Object.Instantiate
            (
                data.Config.CameraPrefab,
                null
            );

            camera.Follow = heroView.FollowTarget;
            camera.LookAt = heroView.LookTarget;

            return camera;
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