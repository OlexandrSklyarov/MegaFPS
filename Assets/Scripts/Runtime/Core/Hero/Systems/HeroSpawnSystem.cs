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
            var gameConfig = ServicesPool.Instance.GetService<GameConfig>();        

            var entity = world.NewEntity();

            var heroView = GetHeroView(gameConfig.HeroPrefab, data.WorldData.HeroSpawnPoint);
            heroView.Init(entity, world);            
            
            //hero
            ref var hero = ref world.GetPool<HeroComponent>().Add(entity);
            hero.ViewRef = heroView;

            //engine
            ref var engine = ref world.GetPool<CharacterEngineComponent>().Add(entity);
            engine.RB = heroView.RB;

            //config
            ref var config = ref world.GetPool<CharacterConfigComponent>().Add(entity);
            config.Prm = heroView.Config;
            
            //input
            world.GetPool<InputComponent>().Add(entity);

            //look 
            ref var look = ref world.GetPool<HeroLookComponent>().Add(entity);
            look.Body = heroView.transform;
            look.HeadRoot = heroView.HeadRoot;
            look.OriginBodyRotation = heroView.transform.rotation;  
            look.OriginCameraRotation = heroView.FPSHeroCameraTarget.localRotation;  

            //fps camera transform
            ref var camTR = ref world.GetPool<FPSCameraTransformComponent>().Add(entity);
            camTR.Value = heroView.FPSHeroCameraTarget; 

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
            pickupWeaponEvent.Type = gameConfig.HeroStartWeapon;
        }   
       

        private HeroView GetHeroView(HeroView heroPrefab, Transform heroSpawnPoint)
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