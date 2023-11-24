using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class Decal : MonoBehaviour, IPoolable<Decal>
    {
        private IObjectPool<Decal> _pool;

        public void SetPool(IObjectPool<Decal> pool)
        {
            _pool = pool;
        }

        public void SetPoint(Vector3 normal, Vector3 point)
        {
            transform.rotation = Quaternion.LookRotation(normal);
            transform.position = point;
            ReleaseOverTimeAsync();
        }

        private async void ReleaseOverTimeAsync()
        {
            await Awaitable.WaitForSecondsAsync(3f);
            Release();
        }

        private void Release()
        {
            _pool.Release(this);
        }
    }
}