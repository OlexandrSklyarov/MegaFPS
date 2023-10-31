using NaughtyAttributes;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "SO/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {        
        [field: Header("Movement"), SerializeField, Min(1f)] public float WalkSpeed {get; private set;} = 2f;
        [field: SerializeField, Min(1f)] public float RunSpeed {get; private set;} = 6f;
        [field: SerializeField, Min(0.01f)] public float MinSpeed {get; private set;} = 0.1f;
        [field: SerializeField, Min(1f)] public float JumpForce {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public LayerMask HeroLayer {get; private set;}        

        [field: Space, BoxGroup("Camera"), SerializeField, Min(0.1f)] public float MouseSensitivity {get; private set;} = 22f;
        [field: SerializeField] public float UpAngle {get; private set;} = -70f;
        [field: SerializeField] public float DownAngle {get; private set;} = 70f;
       
        [field: Space, Header("Foot step"),SerializeField, Min(0.01f)] public float FootSteVelocityThreshold {get; private set;} = 2f;
        [field: SerializeField, Min(0.01f)] public float SprintStepInterval {get; private set;} = 0.2f;
        [field: SerializeField, Min(0.01f)] public float WalkStepInterval {get; private set;} = 0.4f;
        
        [field: Space, BoxGroup("Attack"), SerializeField, Min(0.01f)] public float MelleAttackCooldown {get; private set;} = 1.5f;
        
        [field: Space, BoxGroup("Animation"), SerializeField, Min(1f)] public float AnimationBlendSpeed {get; private set;} = 8.9f;
    }
}