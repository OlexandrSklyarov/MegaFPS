using System.Collections.Generic;

namespace SA.FPS
{
    public class PoolManager : IPoolManager
    {
        private Dictionary<DecalType, DecalPool> DecalPools
        {
            get
            {
                if (decalPools == null)
                {
                    decalPools = new Dictionary<DecalType, DecalPool>();

                    foreach(var item in _config.Decals)
                    {
                        decalPools.Add(item.Type, new DecalPool(item.Prefab, item.StartCount, item.MaxPoolCount));
                    }
                }

                return decalPools;
            }
        }
        
        private Dictionary<DecalType, DecalPool> decalPools;
        private PoolObjectConfig _config;

        public PoolManager(PoolObjectConfig config)
        {
            _config = config;
        }

        public Decal GetDecal(DecalType type) => DecalPools[type].GetDecal();
    }
}