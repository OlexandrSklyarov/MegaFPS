
using UnityEngine;

namespace SA.FPS
{
    public struct OverlapDamageEvent
    {
        public Transform DamageSource;
        public int Damage;
        public float Power;
        public bool IsApplyPushForce;
    }
}