using FMODUnity;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroFootStepAudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<HeroFootStepComponent> _footStepPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<CharacterEngineComponent> _enginePool;
        private EcsPool<InputComponent> _inputPool;

        public void Init(IEcsSystems systems)
        {
            _data = systems.GetShared<SharedData>();
            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<HeroFootStepComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<CharacterEngineComponent>()
                .Inc<InputComponent>()
                .End();

            var world = systems.GetWorld();
            _footStepPool = world.GetPool<HeroFootStepComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _enginePool = world.GetPool<CharacterEngineComponent>();
            _inputPool = world.GetPool<InputComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var footStep = ref _footStepPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var engine = ref _enginePool.Get(ent);
                ref var input = ref _inputPool.Get(ent);

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