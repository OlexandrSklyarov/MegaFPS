using System;
using Cinemachine;
using UnityEngine;

namespace SA.FPS
{
    [Serializable]
    public sealed class WorldData
    {
        [field: SerializeField] public Transform HeroSpawnPoint {get; private set;}
        [field: SerializeField] public HUDController HUD {get; private set;}
        [field: SerializeField] public CinemachineVirtualCamera FPSVirtualCamera {get; private set;}
    }
}