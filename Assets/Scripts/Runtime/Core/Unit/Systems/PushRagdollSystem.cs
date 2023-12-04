using Leopotam.EcsLite;

namespace SA.FPS
{
    public sealed class PushRagdollSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<RagdollComponent> _ragdollPool;
        private EcsPool<PushRagdollEvent> _ragdollEventPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world
                .Filter<RagdollComponent>()
                .Inc<PushRagdollEvent>()
                .End();           

            _ragdollPool = _world.GetPool<RagdollComponent>();
            _ragdollEventPool = _world.GetPool<PushRagdollEvent>();
        }


        public void Run(IEcsSystems systems)
        {  
            foreach(var ent in _filter)
            {
                ref var ragdoll = ref _ragdollPool.Get(ent);
                ref var pushEvt = ref _ragdollEventPool.Get(ent);
              
                ragdoll.Controller.OnAndPushAtPosition
                (
                    pushEvt.HitDirection, 
                    pushEvt.HitPoint,
                    pushEvt.Power
                );

                _ragdollEventPool.Del(ent);
            }
        }
    }
}