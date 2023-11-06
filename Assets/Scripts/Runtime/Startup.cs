using UnityEngine;
using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class Startup : MonoBehaviour
    {     
        [Space, SerializeField] private WorldData _worldData;       

        private EcsWorld _world;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        private void Start()
        {
            var data = new SharedData()
            {
                WorldData = _worldData
            };

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world, data);
            _fixedUpdateSystems = new EcsSystems(_world, data);
            _lateUpdateSystems = new EcsSystems(_world, data);

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
                .Add(new HeroCheckGroundSystem())
                .Add(new HeroInputSystem())
                .Add(new HeroJumpingSystem())
                .Add(new HeroAttackSystem())
                .Add(new HeroFootStepAudioSystem())
                .Add(new HeroShootShakeFXSystem())
                .Add(new HeroAnimationSystem())
                .Add(new HeroPickupWeaponSystem())                
                .Add(new HeroSwitchWeaponSystem()) 
                .Add(new HeroReloadWeaponSystem())   

                .Add(new WeaponMelleAttackSystem())
                .Add(new WeaponShootingSystem())
                .Add(new WeaponReloadSystem())

                .Add(new UpdateHUDSystem())

                .Init();

            
            _fixedUpdateSystems 
                .Add(new HeroPhysicsMovementSystem())
                .Init();

            _lateUpdateSystems
                .Add(new HeroFPSLookCameraSystem())
                .Init();
        }


        private void Update() => _updateSystems?.Run();   


        private void FixedUpdate() => _fixedUpdateSystems?.Run(); 


        private void LateUpdate() => _lateUpdateSystems?.Run(); 


        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _updateSystems = null;   

            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;  

            _lateUpdateSystems?.Destroy();
            _lateUpdateSystems = null;        

            _world?.Destroy();
            _world = null;
        }
    }
}
