using UnityEngine;

namespace SA.FPS
{
    public interface IDamageable
    {
        void ApplyDamage(int damage, Vector3 source = default);
    }
}