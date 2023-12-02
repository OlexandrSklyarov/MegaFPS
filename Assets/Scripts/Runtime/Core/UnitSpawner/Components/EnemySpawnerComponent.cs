
using System.Collections.Generic;
using UnityEngine;

namespace SA.FPS
{
    public struct EnemySpawnerComponent
    {
        public float NextCheckSpawnTime;
        public int EnemiesTypeCount;
        public Queue<int> RandomPoints;
    }
}