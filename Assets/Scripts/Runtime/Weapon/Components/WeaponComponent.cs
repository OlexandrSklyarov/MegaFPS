
using UnityEngine;

namespace SA.FPS
{
    public struct WeaponComponent
    {
        public Transform Center;        
        public Transform FirePoint;         
        public WeaponSettings Settings;
        public float CurrentCooldown;
    }
}