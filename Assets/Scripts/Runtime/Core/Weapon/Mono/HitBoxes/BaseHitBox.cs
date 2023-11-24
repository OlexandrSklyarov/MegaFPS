using UnityEngine;
using Leopotam.EcsLite;
using Runtime.Extensions;

namespace SA.FPS
{
    public abstract class BaseHitBox : MonoBehaviour
    {
        protected EcsWorld World {get; private set;}

        private EcsPackedEntity _unitEntity;

        public void Setup(EcsWorld world, EcsPackedEntity unitEntity)
        {
            World = world;
            _unitEntity = unitEntity;
        }

        protected bool TryGetEntity(out int ent)
        {
            return _unitEntity.Unpack(World, out ent);
        }

        protected ref OverlapDamageEvent GetOverlapDamageEvent(int entity)
        {     
            return ref World.GetOrAddComponent<OverlapDamageEvent>(entity);
        }

        protected ref RaycastDamageEvent GetRaycastDamageEvent(int entity)
        {    
            return ref World.GetOrAddComponent<RaycastDamageEvent>(entity);
        }
    }
}