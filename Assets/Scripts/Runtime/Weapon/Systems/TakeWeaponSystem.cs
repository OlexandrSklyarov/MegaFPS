using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class TakeWeaponSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world
                .Filter<TakeWeaponEvent>()
                .End();

            var evtPool = world.GetPool<TakeWeaponEvent>();

            foreach(var ent in filter)
            {
                ref var takeEvt = ref evtPool.Get(ent);

                CreateWeaponEntity(world, ref takeEvt);

                evtPool.Del(ent);
            }
        }

        private void CreateWeaponEntity(EcsWorld world, ref TakeWeaponEvent takeEvt)
        {
            var ent = world.NewEntity();

            //owner
            ref var weaponOwner = ref world.GetPool<WeaponOwnerComponent>().Add(ent);
            weaponOwner.MyOwner = takeEvt.OwnerEntity;
            
            //weapon
            ref var weapon = ref world.GetPool<WeaponComponent>().Add(ent);            
            weapon.Settings = takeEvt.WeaponView.Settings;
            weapon.FirePoint = takeEvt.WeaponView.FirePoint;
            weapon.Center = takeEvt.WeaponView.transform;
            weapon.AnimatorRef = takeEvt.WeaponView.WeaponAnimator;

            //ammo
            ref var ammunition = ref world.GetPool<AmmunitionComponent>().Add(ent); 
            ammunition.Count = takeEvt.WeaponView.Settings.StartAmmo;
            UnityEngine.Debug.Log(ammunition.Count);            
        }
    }
}