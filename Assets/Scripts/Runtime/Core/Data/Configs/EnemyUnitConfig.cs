using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "EnemyUnitConfig", menuName = "SO/EnemyUnitConfig")]
    public sealed class EnemyUnitConfig : ScriptableObject, IService
    {
        [field: Header("Navigation"), SerializeField, Min(1f)] public float MaxSpeed {get; private set;} = 3f;     
        [field: SerializeField, Min(0.1f)] public float MinSpeed {get; private set;} = 0.5f;     
        [field: SerializeField, Min(0.01f)] public float UpdateDestinationDelay {get; private set;} = 0.5f;     
        [field: Header("Attack"), Space, SerializeField, Min(1f)] public float AttackRange {get; private set;} = 2f;
        [field: SerializeField, Min(0.1f)] public float[] AttackTimeCollection {get; private set;}    

    }
}