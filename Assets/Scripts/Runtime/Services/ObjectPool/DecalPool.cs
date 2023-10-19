using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class DecalPool
    {
        private readonly Decal _prefab;
        private readonly Transform _container;
        private readonly IObjectPool<Decal>_innerPool;

        public DecalPool(Decal prefab, int startCount, int maxCount)
        {            
            _prefab = prefab;
            _container = new GameObject("[DecalPool]").transform;
            
            _innerPool = new ObjectPool<Decal>
            (
                OnCreateItem, OnTakeItem, OnReturnItem, OnDestroyItem, true, startCount, maxCount
            );            
        }

        private void OnDestroyItem(Decal decal)
        {
            UnityEngine.Object.Destroy(decal.gameObject);
        }

        private void OnReturnItem(Decal decal)
        {
            decal.gameObject.SetActive(false);
        }

        private void OnTakeItem(Decal decal)
        {
            decal.gameObject.SetActive(true);
        }

        private Decal OnCreateItem()
        {
            var item = UnityEngine.Object.Instantiate(_prefab, _container);
            (item as IPoolable<Decal>).SetPool(_innerPool);
            return item;            
        }

        public Decal GetDecal()
        {
            return _innerPool.Get();
        }
    }
}