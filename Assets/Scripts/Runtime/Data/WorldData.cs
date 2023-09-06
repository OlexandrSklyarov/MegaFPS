using System;
using UnityEngine;

namespace SA.FPS
{
    [Serializable]
    public sealed class WorldData
    {
        [field: SerializeField] public Transform HeroSpawnPoint {get; private set;}        
    }
}