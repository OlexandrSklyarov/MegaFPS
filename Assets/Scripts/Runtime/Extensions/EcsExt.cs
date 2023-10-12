using Leopotam.EcsLite;

namespace Runtime.Extensions
{
    public static class EcsExt
    {
        /// <summary>
        /// Returns a component <T> by type from the entity, 
        /// if there is no such component, adds it to the entity.
        /// </summary>
        public static ref T GetOrAddComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();

            if (pool.Has(entity)) 
            {
                return ref pool.Get(entity);
            }   
            
            return ref pool.Add(entity); 
        }
    }
}