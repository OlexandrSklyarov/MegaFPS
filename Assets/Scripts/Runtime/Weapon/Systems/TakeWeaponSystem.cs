using Leopotam.EcsLite;
using Util;

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
                DebugUtility.Print("Take weapon event... ");
                evtPool.Del(ent);
            }
        }       
    }
}