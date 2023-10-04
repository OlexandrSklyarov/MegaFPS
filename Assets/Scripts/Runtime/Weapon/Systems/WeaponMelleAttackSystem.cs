using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponMelleAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _attackFilter;  
        private EcsFilter _weaponFilter;  

        public void Init(IEcsSystems systems)
        {
            _attackFilter = systems.GetWorld()
                .Filter<CharacterTryMelleAttackEvent>()
                .End();

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var evtPool = world.GetPool<CharacterTryMelleAttackEvent>();
            var weaponPool = world.GetPool<WeaponComponent>();
            var ownerPool = world.GetPool<WeaponOwnerComponent>();

            //EVENT
            foreach(var s in _attackFilter)
            {
                ref var evt = ref evtPool.Get(s);                

                //WEAPON
                foreach(var w in _weaponFilter)
                {
                    ref var weapon = ref weaponPool.Get(w);
                    ref var owner = ref ownerPool.Get(w);

                    //skip
                    if (owner.MyOwner != evt.AttackEntity) continue;                                                            
                    
                    Attack(ref weapon);
                                        
                }

                evtPool.Del(s);  
            }
        }

        private void Attack(ref WeaponComponent weapon)
        {
            Util.DebugUtility.Print("Attack");
            weapon.AnimatorRef.SetTrigger("MELLE_ATTACK");
        }
    }
}