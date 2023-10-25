using UnityEngine;

namespace SA.FPS
{
    public class FireWeaponView : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePoint {get; private set;}
        [field: SerializeField] public WeaponSettings Settings {get; private set;}        
        [field: SerializeField] public Animator WeaponAnimator {get; private set;}          
    }
}