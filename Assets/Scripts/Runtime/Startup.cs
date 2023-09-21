using UnityEngine;
using Leopotam.EcsLite;
using System;

namespace SA.FPS
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;      
        [Space, SerializeField] private WorldData _worldData;       

        private EcsWorld _world;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;

        private void Start()
        {
            var data = new SharedData()
            {
                Config = _config,
                WorldData = _worldData,
                Services = new ServicesPool()     
            };

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world, data);
            _fixedUpdateSystems = new EcsSystems(_world, data);

            PrepareWorld(); 
            AddSystems();  
        }


        private void PrepareWorld()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        private void AddSystems()
        {
            _updateSystems  
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            #endif
                .Add(new HeroSpawnSystem())
                .Add(new HeroInputSystem())
                .Add(new HeroJumpingSystem())
                .Add(new HeroMovementSystem())
                .Add(new HeroFPSLookCameraSystem())
                .Add(new HeroFootStepAudioSystem())
                .Add(new HeroAnimationSystem())
                .Init();

            
            _fixedUpdateSystems 
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            #endif
                .Init();
        }


        private void Update() => _updateSystems?.Run();   


        private void FixedUpdate() => _fixedUpdateSystems?.Run(); 


        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _updateSystems = null;   

            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;         

            _world?.Destroy();
            _fixedUpdateSystems?.Destroy();
            _world = null;
        }
    }
}
