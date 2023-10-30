using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponMelleAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _attackFilter;  
        private EcsFilter _weaponFilter;
        private EcsPool<CharacterTryMelleAttackEvent> _evtPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;

        public void Init(IEcsSystems systems)
        {
            _attackFilter = systems.GetWorld()
                .Filter<CharacterTryMelleAttackEvent>()
                .End();

            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .End();

            var world = systems.GetWorld();
            _evtPool = world.GetPool<CharacterTryMelleAttackEvent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
        }

        public void Run(IEcsSystems systems)
        {          
            //EVENT
            foreach(var s in _attackFilter)
            {
                ref var evt = ref _evtPool.Get(s);                

                //WEAPON
                foreach(var w in _weaponFilter)
                {
                    ref var weapon = ref _weaponPool.Get(w);
                    ref var owner = ref _ownerPool.Get(w);

                    //skip
                    if (owner.MyOwnerEntity != evt.AttackEntity) continue;                                                            
                    
                    Attack(ref weapon);                                        
                }

                _evtPool.Del(s);  
            }
        }


        private void Attack(ref WeaponComponent weapon)
        {
            Util.DebugUtility.Print("Attack");
        }
    }
}