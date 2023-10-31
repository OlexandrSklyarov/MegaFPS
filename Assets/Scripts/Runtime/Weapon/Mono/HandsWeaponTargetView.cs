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
            _firePoint.SetLocalPositionAndRotation(weaponView.FirePoint.localPosition, weaponView.FirePoint.localRotation);
            _leftHandTarget.SetLocalPositionAndRotation(weaponView.LeftHand.localPosition, weaponView.LeftHand.localRotation);
            _rightHandTarget.SetLocalPositionAndRotation(weaponView.RightHand.localPosition, weaponView.RightHand.localRotation);           
        }        
    }
}