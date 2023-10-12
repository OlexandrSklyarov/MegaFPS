using Leopotam.EcsLite;
using Runtime.Extensions;
using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;  

        public void Init(IEcsSystems systems)
        {            
            _filter = systems.GetWorld()
                .Filter<HeroComponent>()
                .Inc<InputComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterConfigComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var attackPool = world.GetPool<CharacterAttackComponent>();
            var configPool = world.GetPool<CharacterConfigComponent>();
            var inputPool = world.GetPool<InputComponent>();

            foreach(var ent in _filter)
            {
                ref var input = ref inputPool.Get(ent);
                ref var attack = ref attackPool.Get(ent);
                ref var config = ref configPool.Get(ent);

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
                    var type = weapon.CurrentWeaponType;
                    var weaponEntity = weapon.MyWeapons[type];

                    CreateShootEvent(world, weaponEntity);     
                }           
            }
        }

        private void CreateWeaponMelleAttack(EcsWorld world, int attackEntity)
        {
            var ent = world.NewEntity();
            ref var evt = ref world.GetPool<CharacterTryMelleAttackEvent>().Add(ent);
            evt.AttackEntity = attackEntity;
        }

        private void CreateShootEvent(EcsWorld world, int weaponEntity)
        {
            world.GetOrAddComponent<TryShootComponent>(weaponEntity);
        }
    }
}