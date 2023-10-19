using System.Collections.Generic;
using UnityEngine;

namespace SA.FPS
{    
    [CreateAssetMenu(fileName = "PoolObjectConfig", menuName = "SO/PoolObjectConfig")]
    public class PoolObjectConfig : ScriptableObject 
    {
        [field: SerializeField] public List<DecalItem> Decals {get; private set;}

        [System.Serializable]
        public class DecalItem
        {
            [field: SerializeField] public DecalType Type {get; private set;}
            [field: SerializeField] public Decal Prefab {get; private set;}
            [field: SerializeField, Min(4)] public int StartCount {get; private set;} = 10;
            [field: SerializeField, Min(4)] public int MaxPoolCount {get; private set;} = 32;
        }
    }   
}