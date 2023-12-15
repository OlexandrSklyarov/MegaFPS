using UnityEngine;
using UnityEngine.Pool;

namespace SA.FPS
{
    public class Decal : MonoBehaviour, IPoolable<Decal>
    {
        [SerializeField] private ParticleSystem _impactVFX;

        private IObjectPool<Decal> _pool;

        public void SetPool(IObjectPool<Decal> pool)
        {
            _pool = pool;
        }

        public void SetPoint(Vector3 normal, Vector3 point)
        {
            transform.rotation = Quaternion.LookRotation(normal);
            transform.position = point;

            if (_impactVFX != null) _impactVFX.Play();

            ReleaseOverTimeAsync();
        }

        private async void ReleaseOverTimeAsync()
        {
            await Awaitable.WaitForSecondsAsync(3f);
            Release();
        }

        private void Release()
        {
            if (_impactVFX != null) _impactVFX.Stop();
            _pool.Release(this);
        }
    }
}