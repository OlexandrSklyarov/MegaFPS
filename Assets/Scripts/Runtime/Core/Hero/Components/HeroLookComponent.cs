using UnityEngine;

namespace SA.FPS
{
    public struct HeroLookComponent
    {
        public Transform Body;
        public Transform HeadRoot;
        public Quaternion OriginBodyRotation;
        public Quaternion OriginCameraRotation;
        public float VerticalRotationAngle;
        public float HorizontalRotationAngle;
    }
}