using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class WeaponShootingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IPoolManager _poolManager;
        private EcsFilter _weaponFilter;
        private EcsPool<TryShootComponent> _tryShootEvtPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;
        private EcsPool<AmmunitionComponent> _ammoPool;

        public void Init(IEcsSystems systems)
        {     
            _poolManager = ServicesPool.Instance.GetService<IPoolManager>();

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<AmmunitionComponent>()
                .Inc<TryShootComponent>()
                .End();

            var world = systems.GetWorld();
            _tryShootEvtPool = world.GetPool<TryShootComponent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
            _ammoPool = world.GetPool<AmmunitionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            foreach(var ent in _weaponFilter)
            {
                ref var weapon = ref _weaponPool.Get(ent);
                ref var owner = ref _ownerPool.Get(ent);
                ref var ammo = ref _ammoPool.Get(ent);                 
                ref var shootEvt = ref _tryShootEvtPool.Get(ent);                 
                                
                if (weapon.WeaponReadyTime < Time.time && ammo.Count > 0) //fire                
                {                    
                    Shoot(ref weapon, ref ammo, ref shootEvt);
                    AddOwnerShakeFXComponent(world, ref owner, ref weapon);  

                    if (ammo.Count <= 0)
                    {
                        world.GetOrAddComponent<WeaponReloadEvent>(ent);   
                    }                                                         
                }

                //update ui event
                world.GetOrAddComponent<WeaponChangeStateComponentTag>(ent);    

                _tryShootEvtPool.Del(ent);                                     
            }         
        }


        private void Shoot(ref WeaponComponent weapon, ref AmmunitionComponent ammo, ref TryShootComponent shootEvt)
        {
            for (int i = 0; i < weapon.View.Settings.RayCountPerShoot; i++)
            {
                var shootDir = TryGetShootDirectionWithSpread(ref weapon, ref shootEvt);

                if (Physics.Raycast(shootEvt.ShootPoint, shootDir, out var hit, float.MaxValue, weapon.View.Settings.TargetLayerMask))
                {
                    HitScan(ref weapon, hit);
                }
            }

            ammo.Count--;

            weapon.WeaponReadyTime = Time.time + weapon.View.Settings.AttackCooldown;

            FMODUnity.RuntimeManager.PlayOneShot(weapon.View.Settings.AttackSfx);  
        }


        private Vector3 TryGetShootDirectionWithSpread(ref WeaponComponent weapon, ref TryShootComponent shootEvt)
        {
            return (weapon.View.Settings.IsUseSpread) ?
                AddShootSpread(weapon.View.Settings.SpreadFactor, shootEvt.Direction) :
                shootEvt.Direction;
        }


        private void HitScan(ref WeaponComponent weapon, RaycastHit hit)
        {            
            if (hit.collider.TryGetComponent(out IAttackVisitor target))
            {
                Accept(ref weapon, target, hit);
            }
            else
            {
                NoTargetHit(ref weapon, ref hit);
            }
        }


        private void Accept(ref WeaponComponent weapon,  IAttackVisitor target, RaycastHit hit)
        {
            Util.Debug.Print("Accept");
            target.Visit(weapon.View, hit);
        }


        private void NoTargetHit(ref WeaponComponent weapon, ref RaycastHit hit)
        {
            var decal = _poolManager.GetDecal(weapon.View.Settings.DecalType);
            decal.SetPoint(hit.normal, hit.point);
        }

        private Vector3 AddShootSpread(float spread, Vector3 dir)
        {
            var modifierDir =  dir + new Vector3
            (
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread),
                UnityEngine.Random.Range(-spread, spread)
            );

            return modifierDir.normalized;
        }


        private void AddOwnerShakeFXComponent(EcsWorld world, ref WeaponOwnerComponent owner, ref WeaponComponent weapon)
        {    
            ref var shake = ref world.GetOrAddComponent<CameraShakeComponent>(owner.MyOwnerEntity);  

            shake.Duration = weapon.View.Settings.ShakeDuration;
            shake.Strength = weapon.View.Settings.Strength;
            shake.Vibrato = weapon.View.Settings.Vibrato;
            shake.Randomness = weapon.View.Settings.Randomness;
        }
    }
}