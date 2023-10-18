using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "SO/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {        
        [field: Header("Movement"), SerializeField, Min(1f)] public float WalkSpeed {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public float RunMultiplier {get; private set;} = 2f;
        [field: SerializeField, Min(1f)] public float JumpForce {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public float Gravity {get; private set;} = 9.81f;

        
        [field: Space, Header("Foot step"),SerializeField, Min(0.01f)] public float FootSteVelocityThreshold {get; private set;} = 2f;
        [field: SerializeField, Min(0.01f)] public float SprintStepInterval {get; private set;} = 0.2f;
        [field: SerializeField, Min(0.01f)] public float WalkStepInterval {get; private set;} = 0.4f;

        
        [field: Space, Header("Attack"), SerializeField, Min(0.01f)] public float MelleAttackCooldown {get; private set;} = 1.5f;
    }
}