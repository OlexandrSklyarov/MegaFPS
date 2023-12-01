using UnityEngine;

namespace SA.FPS
{
    public struct HeroLookComponent
    {
        public Transform Body;
        public Transform HeadRoot;
        public Transform Head;
        public Transform FPS_CameraTarget;
        public float VerticalRotation;
        public float HorizontalRotation;
    }
}