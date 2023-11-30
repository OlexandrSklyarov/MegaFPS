using Runtime.Services.WeaponsFactory;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SO/GameConfig")]
    public sealed class GameConfig : ScriptableObject, IService
    {
        [field: Header("Hero"), SerializeField] public HeroView HeroPrefab {get; private set;}
        [field: SerializeField] public WeaponType HeroStartWeapon {get; private set;}        
        [field: Space, SerializeField] public GameAudioConfig Audio {get; private set;}
        [field: Space, SerializeField] public WeaponsConfig WeaponData {get; private set;}
        [field: Space, SerializeField] public PoolObjectConfig PoolData {get; private set;}        
    }
}