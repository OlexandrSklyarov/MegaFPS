using Leopotam.EcsLite;
using Runtime.Extensions;
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

        public void Init(IEcsSystems systems)
        {            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<InputComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<HeroLookComponent>()
                .End();
            
            var world = systems.GetWorld();
            _attackPool = world.GetPool<CharacterAttackComponent>();
            _configPool = world.GetPool<CharacterConfigComponent>();
            _inputPool = world.GetPool<InputComponent>();
            _lookPool = world.GetPool<HeroLookComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);
                ref var attack = ref _attackPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var look = ref _lookPool.Get(ent);

                if (attack.MelleAttackTimer > 0f)
                {
                    attack.MelleAttackTimer -= Time.deltaTime;
                    continue;
                }

                if (input.IsAttack)
                {
                    CreateWeaponMelleAttack(world, ent);                    
                    attack.MelleAttackTimer = config.Prm.MelleAttackCooldown;
                    continue;
                }

                var pool = world.GetPool<HasWeaponComponent>();

                //is can shoot
                if (input.IsFire && pool.Has(ent)) 
                {
                    ref var weapon = ref pool.Get(ent);
                    var type = weapon.CurrentUsedWeaponType;
                    var weaponEntity = weapon.MyWeaponCollections[type];

                    CreateShootEvent(world, weaponEntity, ref look);     
                }           
            }
        }


        private void CreateWeaponMelleAttack(EcsWorld world, int attackEntity)
        {
            var ent = world.NewEntity();
            ref var evt = ref world.GetPool<CharacterTryMelleAttackEvent>().Add(ent);
            evt.AttackEntity = attackEntity;
        }


        private void CreateShootEvent(EcsWorld world, int weaponEntity, ref HeroLookComponent look)
        {
            ref var evt = ref world.GetOrAddComponent<TryShootComponent>(weaponEntity);
            evt.ShootPoint = look.FPS_Camera.transform.position;
            evt.Direction = look.FPS_Camera.transform.forward;
        }
    }
}