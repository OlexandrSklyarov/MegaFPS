using UnityEngine;

namespace SA.FPS
{
    public struct EnemyAttackComponent
    {
        public float AttackCooldown;
        public bool IsAttackStarted;
        public int AttackIndex;
        public Vector3 TargetPos;
    }
}