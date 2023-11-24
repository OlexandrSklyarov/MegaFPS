using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class WeaponMeleeAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _weaponFilter;
        private EcsPool<WeaponMeleeAttackEvent> _evtPool;
        private EcsPool<WeaponComponent> _weaponPool;
        private EcsPool<WeaponOwnerComponent> _ownerPool;

        public void Init(IEcsSystems systems)
        {
            _weaponFilter = systems.GetWorld()
                .Filter<WeaponComponent>()
                .Inc<WeaponOwnerComponent>()
                .Inc<WeaponMeleeAttackEvent>()
                .End();

            var world = systems.GetWorld();
            _evtPool = world.GetPool<WeaponMeleeAttackEvent>();
            _weaponPool = world.GetPool<WeaponComponent>();
            _ownerPool = world.GetPool<WeaponOwnerComponent>();
        }

        public void Run(IEcsSystems systems)
        {                
            foreach(var ent in _weaponFilter)
            {
                _evtPool.Del(ent); 

                ref var weapon = ref _weaponPool.Get(ent);
                ref var owner = ref _ownerPool.Get(ent);
                                                                            
                Attack(ref weapon);                                        
            }             
        }


        private void Attack(ref WeaponComponent weapon)
        {
            weapon.View.MeleeAttack(out float duration);
        }
    }
}