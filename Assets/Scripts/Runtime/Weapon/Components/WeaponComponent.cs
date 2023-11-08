
using UnityEngine;

namespace SA.FPS
{
    public struct WeaponComponent
    {
        public Transform Center;        
        public Transform FirePoint;      
        public IWeaponView View;
        public float WeaponReadyTime;

    }
}