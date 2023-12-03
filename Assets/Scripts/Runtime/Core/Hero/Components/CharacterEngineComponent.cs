using UnityEngine;

namespace SA.FPS
{
    public struct CharacterEngineComponent
    {
        public Rigidbody RB;
        public bool IsGrounded;
        public float NextJumpTime;
        public float CurrentSpeed;
        public float CurrentSmoothVelocity;
    }
}