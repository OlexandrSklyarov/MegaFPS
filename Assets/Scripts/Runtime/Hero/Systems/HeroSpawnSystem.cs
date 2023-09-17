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

            //tps camera
            ref var tpsCamera = ref world.GetPool<TPSCameraComponent>().Add(entity);
            tpsCamera.Virtual = GetCamera(data, heroView);
            tpsCamera.TPS_Camera = GetTPSCamera(data);

            //input
            world.GetPool<InputComponent>().Add(entity);

            //look (fps camera)
            ref var look = ref world.GetPool<CharacterLookComponent>().Add(entity);
            look.Body = heroView.transform;
            look.FPS_Camera = heroView.HeroCamera;
        }

        private TPSCamera GetTPSCamera(SharedData data)
        {
            var camera = UnityEngine.Object.Instantiate
            (
                data.Config.TPSCameraPrefab,
                null
            );

            camera.Off();
            return camera;
        }

        private CinemachineVirtualCamera GetCamera(SharedData data, HeroView heroView)
        {
            var camera = UnityEngine.Object.Instantiate
            (
                data.Config.VirtualCameraPrefab,
                null
            );

            camera.Follow = heroView.FollowTarget;
            camera.LookAt = heroView.LookTarget;
            camera.enabled = false;

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