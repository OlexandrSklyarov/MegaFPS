using FMODUnity;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFootStepAudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {
            _data = systems.GetShared<SharedData>();
            
            _filter = systems.GetWorld()
                .Filter<HeroTag>()
                .Inc<HeroFootStepComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<InputComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var footStepPool = world.GetPool<HeroFootStepComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var enginePool = world.GetPool<CharacterEngineComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var footStep = ref footStepPool.Get(ent);
                ref var config = ref configPool.Get(ent);
                ref var engine = ref enginePool.Get(ent);
                ref var input = ref inputPool.Get(ent);

                var sprintInterval = (input.IsRun) ? config.Prm.SprintStepInterval : config.Prm.WalkStepInterval;

                if (IsPlayStepTime(ref engine, ref footStep, ref config))
                {
                    RuntimeManager.PlayOneShot(_data.Config.Audio.Hero.FootSteps);
                    footStep.NextStepTime = Time.time + sprintInterval;
                }    
            }
        }

        private bool IsPlayStepTime(ref CharacterEngineComponent engine, ref HeroFootStepComponent footStep, ref CharacterConfigComponent config)
        {
            var isGrounded = engine.CharacterController.isGrounded;
            var isMoving = engine.CurrentMovement.x != 0f || engine.CurrentMovement.z != 0f;
            var isNextStep = Time.time > footStep.NextStepTime;
            var isThresholdReached = engine.CharacterController.velocity.magnitude > config.Prm.FootSteVelocityThreshold;
         
            return isGrounded && isMoving && isNextStep && isThresholdReached;
        }        
    }
}