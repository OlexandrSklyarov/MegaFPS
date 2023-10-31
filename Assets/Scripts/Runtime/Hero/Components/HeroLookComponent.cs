
using UnityEngine;

namespace SA.FPS
{
    public struct HeroLookComponent
    {
        public Transform Body;
        public Transform HeadRoot;
        public Transform Head;
        public Transform CameraOrigin;
        public Camera FPS_Camera;
        public float VerticalRotation;
        public float HorizontalVelocity;
    }
}