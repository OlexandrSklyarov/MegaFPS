using System;
using FMODUnity;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "GameAudioConfig", menuName = "SO/GameAudioConfig")]
    public sealed class GameAudioConfig : ScriptableObject   
    {
        [field: SerializeField] public HeroSfx Hero {get; private set;}
        
        [Serializable]
        public sealed class HeroSfx
        {
            [field: SerializeField] public EventReference FootSteps {get; private set;}  
        }
    }
}