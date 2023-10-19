using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public interface IPoolable<T> where T : MonoBehaviour
    {
        void SetPool(IObjectPool<T> pool);
    }
}