using UnityEngine;

namespace SA.FPS
{
    public class HandsWeaponTargetView : MonoBehaviour
    {
        [field: SerializeField] public Transform WeaponsRoot { get; private set; }
        public Transform FirePoint => _firePoint;

        [SerializeField] public Transform _firePoint;
        [SerializeField] public Transform _leftHandTarget;
        [SerializeField] public Transform _rightHandTarget;


        public void SetTargets(IWeaponView weaponView)
        {
            _firePoint.SetPositionAndRotation(weaponView.FirePoint.position, weaponView.FirePoint.rotation);
            _leftHandTarget.SetPositionAndRotation(weaponView.LeftHand.position, weaponView.LeftHand.rotation);
            _rightHandTarget.SetPositionAndRotation(weaponView.RightHand.position, weaponView.RightHand.rotation);           
        }        
    }
}