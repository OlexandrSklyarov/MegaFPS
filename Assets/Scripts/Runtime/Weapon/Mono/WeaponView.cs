using UnityEngine;

namespace SA.FPS
{
    /// <summary>
    /// weapon model teg
    /// </summary>
    public class WeaponView : MonoBehaviour, IWeaponView
    {        
        [field: SerializeField] public Transform FirePoint {get; private set;}
    }
}