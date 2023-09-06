using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "SO/CharacterConfig")]
    public sealed class CharacterConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float WalkSpeed {get; private set;} = 5f;
        [field: SerializeField, Min(1f)] public float RunMultiplier {get; private set;} = 2f;
    }
}