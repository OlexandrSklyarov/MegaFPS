using System;
using Cinemachine;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SO/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] public HeroView HeroPrefab {get; private set;}
        [field: SerializeField] public CinemachineVirtualCamera CameraPrefab  {get; private set;}
        [field: SerializeField] public ControlConfig Control {get; private set;}

        [Serializable]
        public sealed class ControlConfig
        {
            [field: SerializeField, Min(1f)] public float MouseSensitivity {get; private set;} = 2f;
            [field: SerializeField, Min(1f)] public float UpDownAngle {get; private set;} = 80f;
        }
    }
}