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
        
        [field: Space, BoxGroup("Audio"), SerializeField] public GameAudioConfig Audio {get; private set;}
        [field: Space, BoxGroup("Weapons"), SerializeField] public WeaponsConfig WeaponData {get; private set;}
        [field: Space, BoxGroup("Pool GO"), SerializeField] public PoolObjectConfig PoolData {get; private set;}
        
    }
}