using UnityEngine.AI;

namespace SA.FPS
{
    public struct NavigationComponent
    {
        public NavMeshAgent AgentRef;
        public float StopDelay;
        public float NextUpdateDestinationTimer;
        public float Speed;
        public float MaxSpeed;
    }
}