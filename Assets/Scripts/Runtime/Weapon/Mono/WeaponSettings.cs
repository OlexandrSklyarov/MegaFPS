using TriInspector;
using UnityEngine;

namespace SA.FPS
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "SO/WeaponSettings")]
    public class WeaponSettings : ScriptableObject
    {
        [PropertyOrder(0), SerializeField] public float Duration => ShootCooldown;
        [field: PropertyOrder(0), SerializeField] public float Strength {get; private set;} = 1f;
        [field: PropertyOrder(0), SerializeField] public float Randomness {get; private set;} = 90f;
        [field: PropertyOrder(0), SerializeField] public int Vibrato {get; private set;} = 10;
        [field: Space, PropertyOrder(0), SerializeField] public float ShootCooldown {get; private set;} = 0.5f;
        [field: PropertyOrder(0), SerializeField] public int StartAmmo {get; private set;} = 60;
    }
}