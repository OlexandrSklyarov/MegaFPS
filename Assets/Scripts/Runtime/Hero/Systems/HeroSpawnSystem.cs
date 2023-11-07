using Cinemachine;
using Leopotam.EcsLite;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroSpawnSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<SharedData>();  
            var gameConfig = ServicesPool.Instance.GetService<GameConfig>();        

            var entity = world.NewEntity();

            var heroView = GetView(gameConfig.HeroPrefab, data.WorldData.HeroSpawnPoint);
            heroView.Init(entity, world);            
            
            //hero
            ref var hero = ref world.GetPool<HeroComponent>().Add(entity);
            hero.View = heroView;

            //engine
            ref var engine = ref world.GetPool<CharacterEngineComponent>().Add(entity);
            engine.RB = heroView.RB;

            //config
            ref var config = ref world.GetPool<CharacterConfigComponent>().Add(entity);
            config.Prm = heroView.Config;

            //tps camera
            ref var tpsCamera = ref world.GetPool<TPSCameraComponent>().Add(entity);
            tpsCamera.VirtualCamera = GetTPSVirtualCamera(data, heroView, gameConfig);

            //input
            world.GetPool<InputComponent>().Add(entity);

            //look (fps camera)
            ref var look = ref world.GetPool<HeroLookComponent>().Add(entity);
            look.Body = heroView.transform;
            look.HeadRoot = heroView.HeadRoot;
            look.Head = heroView.Head;
            look.FPS_CameraTarget = heroView.FPSHeroCameraTarget;
            SetupFpsVirtualCamera(data, ref look);
            

            //audio
            world.GetPool<HeroFootStepComponent>().Add(entity);
            
            //animation
            ref var anim = ref world.GetPool<CharacterAnimationComponent>().Add(entity);
            anim.HeadAnimatorRef = heroView.HeadAnimator;
            anim.HorPrm = Animator.StringToHash("HOR");
            anim.VertPrm = Animator.StringToHash("VERT");
            anim.SpeedPrm = Animator.StringToHash("SPEED");

            //attack
            ref var attack = ref world.GetPool<CharacterAttackComponent>().Add(entity);

            //Add base weapon
            ref var pickupWeaponEvent = ref world.GetPool<CharacterPickupWeaponEvent>().Add(entity);
            pickupWeaponEvent.Type = WeaponType.Knife;
        }

        private void SetupFpsVirtualCamera(SharedData data, ref HeroLookComponent look)
        {
            var vc = data.WorldData.FPSVirtualCamera;
            vc.Follow = look.FPS_CameraTarget;
            vc.LookAt = look.FPS_CameraTarget;

            look.FPS_VirtualCamera = vc.transform;
            look.VirtualCameraAimPOV = vc.GetCinemachineComponent<CinemachinePOV>();
        }     


        private CinemachineVirtualCamera GetTPSVirtualCamera(SharedData data, HeroView heroView, GameConfig gameConfig)
        {
            var camera = UnityEngine.Object.Instantiate
            (
                gameConfig.VirtualCameraPrefab,
                null
            );

            camera.Follow = heroView.FollowTarget;
            camera.LookAt = heroView.LookTarget;
            camera.Priority = 0;
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

            return hero;
        }
    }
}