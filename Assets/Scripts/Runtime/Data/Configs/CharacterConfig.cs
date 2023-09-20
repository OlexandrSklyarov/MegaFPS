using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "SO/CharacterConfig")]
    public sealed class CharacterConfig : ScriptableObject
    {
        [Header("Movement")]
        [field: SerializeField, Min(1f)] public float WalkSpeed {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public float RunMultiplier {get; private set;} = 2f;
        [field: SerializeField, Min(1f)] public float JumpForce {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public float Gravity {get; private set;} = 9.81f;

        [Header("Foot step")]
        [field: Space, SerializeField, Min(0.01f)] public float FootSteVelocityThreshold {get; private set;} = 2f;
        [field: SerializeField, Min(0.01f)] public float SprintStepInterval {get; private set;} = 0.2f;
        [field: SerializeField, Min(0.01f)] public float WalkStepInterval {get; private set;} = 0.4f;
    }
}