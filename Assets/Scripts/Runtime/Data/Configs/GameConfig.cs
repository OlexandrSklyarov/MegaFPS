using System;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SO/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [field: BoxGroup("Prefabs"), SerializeField] public HeroView HeroPrefab {get; private set;}
        [field: BoxGroup("Prefabs"), SerializeField] public CinemachineVirtualCamera VirtualCameraPrefab  {get; private set;}
        [field: BoxGroup("Prefabs"), SerializeField] public TPSCamera TPSCameraPrefab  {get; private set;}
        
        [field: BoxGroup("Control hero prm"), Space, SerializeField] public ControlConfig Control {get; private set;}
        [field: Space, BoxGroup("Audio"), SerializeField] public GameAudioConfig Audio {get; private set;}
        [field: Space, BoxGroup("Inventory"), SerializeField] public InventoryConfig InventoryData {get; private set;}

        [Serializable]
        public sealed class ControlConfig
        {
            [field: SerializeField, Min(0.1f)] public float MouseSensitivity {get; private set;} = 0.7f;
            [field: SerializeField, Min(1f)] public float UpDownAngle {get; private set;} = 80f;
            [field: SerializeField, Min(0.01f)] public float SmoothRotationTime {get; private set;} = 0.1f;
            [field: SerializeField, Min(0.1f)] public float SmoothVerticalRotation {get; private set;} = 300f;
            [field: SerializeField, Min(0.1f)] public float SmoothHorizontalRotation {get; private set;} = 15f;
        }
    }
}