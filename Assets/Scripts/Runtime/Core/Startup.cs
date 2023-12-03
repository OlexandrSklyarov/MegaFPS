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
            var sharedData = new SharedData()
            {
                WorldData = _worldData
            };

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world, sharedData);
            _fixedUpdateSystems = new EcsSystems(_world, sharedData);
            _lateUpdateSystems = new EcsSystems(_world, sharedData);

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

                //hero
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

                .Add(new WeaponMeleeAttackSystem())
                .Add(new WeaponShootingSystem())
                .Add(new WeaponReloadSystem())

                .Add(new UpdateWeaponHUDSystem())

                // spawn enemy
                .Add(new CreateEnemySpawnerSystem())
                .Add(new UnitSpawnSystem())

                //enemy
                .Add(new UnitApplyDamageSystem())
                .Add(new PushRagdollSystem())
                .Add(new DeathEnemySystem())                  
                .Add(new EnemyAnimationSystem())               

                .Init();
            
            _fixedUpdateSystems 
                .Add(new HeroPhysicsMovementSystem())
                .Init();

            _lateUpdateSystems
                .Add(new HeroLookCameraSystem())
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
