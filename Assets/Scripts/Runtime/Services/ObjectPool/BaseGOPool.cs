using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class BaseGOPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly T _prefab;
        private readonly Transform _container;
        private readonly IObjectPool<T>_innerPool;

        public BaseGOPool(T prefab, int startCount, int maxCount, string poolName)
        {            
            _prefab = prefab;
            _container = new GameObject($"[{poolName}]").transform;
            
            _innerPool = new ObjectPool<T>
            (
                OnCreateItem, OnTakeItem, OnReturnItem, OnDestroyItem, true, startCount, maxCount
            );            
        }

        private void OnDestroyItem(T decal)
        {
            UnityEngine.Object.Destroy(decal.gameObject);
        }

        private void OnReturnItem(T decal)
        {
            decal.gameObject.SetActive(false);
        }

        private void OnTakeItem(T decal)
        {
            decal.gameObject.SetActive(true);
        }

        private T OnCreateItem()
        {
            var item = UnityEngine.Object.Instantiate(_prefab, _container);
            item.SetPool(_innerPool);
            return item;            
        }

        public T Get()
        {
            return _innerPool.Get();
        }
    }
}