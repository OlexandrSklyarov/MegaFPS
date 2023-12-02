using UnityEngine;

namespace SA.FPS
{
    public struct RaycastDamageEvent
    {
        public RaycastHit Hit;
        public int Damage;
        public int DamageMultiplier;
        public float Power;
    }
}