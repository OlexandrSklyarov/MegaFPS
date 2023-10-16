
using UnityEngine;

namespace SA.FPS
{
    public struct HeroLookComponent
    {
        public Transform Body;
        public Transform HeadRoot;
        public Transform Head;
        public Camera FPS_Camera;
        public float VerticalRotation;
        public float VerticalVelocity;
        public float HorizontalVelocity;
    }
}