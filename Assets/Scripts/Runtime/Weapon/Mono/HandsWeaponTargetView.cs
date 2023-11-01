using UnityEngine;

namespace SA.FPS
{
    public class HandsWeaponTargetView : MonoBehaviour
    {
        [field: SerializeField] public Transform WeaponsRoot { get; private set; }
        [SerializeField] public Transform FirePoint { get; private set; }

        public void SetTargets(IWeaponView weapon)
        {
            FirePoint = weapon.FirePoint;
        }
    }
}