using System;
using Leopotam.EcsLite;
using Runtime.Extensions;
using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterAttackComponent> _attackPool;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<InputComponent> _inputPool;
        private EcsPool<HeroLookComponent> _lookPool;
        private EcsPool<HasWeaponComponent> _hasWeaponPool;
        private EcsPool<CharacterAttackEvent> _eventPool;

        public void Init(IEcsSystems systems)
        {            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<InputComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<HeroLookComponent>()
                .Inc<HasWeaponComponent>()
                .Inc<CharacterAttackEvent>()
                .End();
            
            var world = systems.GetWorld();
            _attackPool = world.GetPool<CharacterAttackComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _inputPool = world.GetPool<InputComponent>();
            _lookPool = world.GetPool<HeroLookComponent>();
            _hasWeaponPool = world.GetPool<HasWeaponComponent>();
            _eventPool = world.GetPool<CharacterAttackEvent>();
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                _eventPool.Del(ent);

                ref var input = ref _inputPool.Get(ent);
                ref var attack = ref _attackPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var look = ref _lookPool.Get(ent);
                ref var hasWeapon = ref _hasWeaponPool.Get(ent);

                if (Time.time < attack.EndAttackTime) 
                {
                    continue;
                }

                if (input.IsAttack)
                {
                    MeleeAttack(world, ref config, ref attack, ref hasWeapon);
                    continue;
                }

                if (input.IsFire)
                {
                    if (IsRangeWeapon(ref hasWeapon))
                    {
                        TryWeaponAttack(world, ref look, ref hasWeapon);
                    }
                    else
                    {
                        MeleeAttack(world, ref config, ref attack, ref hasWeapon);
                    }
                }
            }
        }


        private bool IsRangeWeapon(ref HasWeaponComponent hasWeapon)
        {
            return hasWeapon.CurrentUsedWeaponType != WeaponType.Knife;
        }


        private void MeleeAttack(EcsWorld world, ref CharacterConfigComponent config, 
            ref CharacterAttackComponent attack, ref HasWeaponComponent hasWeapon)
        {
            var weaponEntity = hasWeapon.MyWeaponCollections[hasWeapon.CurrentUsedWeaponType];
            world.GetOrAddComponent<WeaponMeleeAttackEvent>(weaponEntity);
            attack.EndAttackTime = Time.time + config.Prm.MeleeAttackCooldown;
        }


        private void TryWeaponAttack(EcsWorld world, ref HeroLookComponent look, ref HasWeaponComponent hasWeapon)
        {    
            var type = hasWeapon.CurrentUsedWeaponType;
            var weaponEntity = hasWeapon.MyWeaponCollections[type];

            CreateShootEvent(world, weaponEntity, ref look);        
        }


        private void CreateShootEvent(EcsWorld world, int weaponEntity, ref HeroLookComponent look)
        {
            ref var evt = ref world.GetOrAddComponent<TryShootComponent>(weaponEntity);
            evt.ShootPoint = look.FPS_CameraTarget.position;
            evt.Direction = look.FPS_CameraTarget.forward;
        }
    }
}