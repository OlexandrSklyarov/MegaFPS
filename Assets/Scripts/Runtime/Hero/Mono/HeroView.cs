using UnityEngine;

namespace SA.FPS
{
    public sealed class HeroView : MonoBehaviour 
    {
        [field: SerializeField] public CharacterConfig Config {get; private set;}
        [field: SerializeField] public CharacterController CharacterController {get; private set;}
        [field: SerializeField] public Transform LookTarget {get; private set;}
        [field: SerializeField] public Camera HeroCamera {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}
        public Transform FollowTarget => transform;

        public void Init()
        {
        }
    }
}